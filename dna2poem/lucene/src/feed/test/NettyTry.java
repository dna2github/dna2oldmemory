package feed.test;

import java.io.File;
import java.io.RandomAccessFile;
import javax.activation.MimetypesFileTypeMap;
// netty-transport.jar + netty-common.jar
import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelOption;
import io.netty.channel.ChannelProgressiveFuture;
import io.netty.channel.ChannelProgressiveFutureListener;
import io.netty.channel.DefaultFileRegion;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import io.netty.channel.socket.SocketChannel;
import io.netty.util.CharsetUtil;
// netty-codec.jar + netty-codec-http.jar
import io.netty.handler.codec.http.HttpContent;
import io.netty.handler.codec.http.HttpRequest;
import io.netty.handler.codec.http.HttpResponseEncoder;
import io.netty.handler.codec.http.HttpRequestDecoder;
import io.netty.handler.codec.http.HttpResponseStatus;
import io.netty.handler.codec.http.HttpVersion;
import io.netty.handler.codec.http.DefaultFullHttpResponse;
import io.netty.handler.codec.http.DefaultHttpResponse;
import io.netty.handler.codec.http.FullHttpResponse;
import io.netty.handler.codec.http.HttpHeaders;
// netty-buffer.jar
import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;

public class NettyTry {
	
	private static class HttpServer {
		public void start(String addr, int port) {
			EventLoopGroup master = new NioEventLoopGroup();
			EventLoopGroup slave = new NioEventLoopGroup();
			try {
				ServerBootstrap server = new ServerBootstrap();
				server.group(master, slave);
				server.channel(NioServerSocketChannel.class);
				server.childHandler(new ChannelInitializer<SocketChannel>() {
					@Override
					protected void initChannel(SocketChannel ch)
							throws Exception {
						ch.pipeline().addLast(new HttpResponseEncoder());
						ch.pipeline().addLast(new HttpRequestDecoder());
						ch.pipeline().addLast(new HttpServerHandler());
					}
				});
				server.option(ChannelOption.SO_BACKLOG, 128);
				server.childOption(ChannelOption.SO_KEEPALIVE, true);
				
				ChannelFuture future = server.bind(addr, port).sync();
				future.channel().closeFuture().sync();
			} catch (Exception e) {
			} finally {
				slave.shutdownGracefully();
				master.shutdownGracefully();
			}
		}
	}
	
	private static class HttpServerHandler extends ChannelInboundHandlerAdapter {
		private static final String template = "<html><body>Hello, %s!</body></html>";
		
		private HttpRequest request;
		private String name;
		
		// HttpServerHandler extends SimpleChannelInboundHandler<FullHttpRequest>
		// => channelRead0(ChannelHandlerContext ctx, FullHttpRequest request)
		@Override
		public void channelRead(ChannelHandlerContext ctx, Object message)
				throws Exception {
			if (message instanceof HttpRequest) {
				request = (HttpRequest)message;
				String uri = request.getUri();
				System.out.println(uri);
				String[] dir = uri.split("/");
				if (dir.length > 1) {
					name = dir[dir.length - 1];
				} else {
					name = "(unknown)";
				}
			}
			if (message instanceof HttpContent) {
				HttpContent content = (HttpContent)message;
				ByteBuf buf = content.content();
				buf.toString(CharsetUtil.UTF_8);
				buf.release();
				byte[] stream = String.format(template, name).getBytes("UTF-8");
				FullHttpResponse response;
				if (name.compareTo("favicon.ico") == 0) {
					response = new DefaultFullHttpResponse(
							HttpVersion.HTTP_1_1,
							HttpResponseStatus.NOT_FOUND);
					response.headers()
						.set(HttpHeaders.Names.CONTENT_TYPE, "text/plain")
						.set(HttpHeaders.Names.CONTENT_LENGTH, response.content().readableBytes());
				} else if (name.compareTo("netty") == 0) {
					response = new DefaultFullHttpResponse(
							HttpVersion.HTTP_1_1,
							HttpResponseStatus.FOUND);
					response.headers().set(HttpHeaders.Names.LOCATION, "/ghost");
				} else if (name.compareTo("self") == 0) {
					System.out.println(new File("test.txt").getAbsolutePath());
					RandomAccessFile file = new RandomAccessFile(new File("test.txt"), "r");
					long filelen = file.length();
					DefaultHttpResponse fileresponse =
							new DefaultHttpResponse(HttpVersion.HTTP_1_1, HttpResponseStatus.OK);
					MimetypesFileTypeMap mimeTypesMap = new MimetypesFileTypeMap();
					fileresponse.headers().set(
							HttpHeaders.Names.CONTENT_TYPE,
							mimeTypesMap.getContentType("test.txt"));
					if (HttpHeaders.isKeepAlive(request)) {
						fileresponse.headers().set(HttpHeaders.Names.CONNECTION, HttpHeaders.Values.KEEP_ALIVE);
					}
					ctx.write(fileresponse);
					ChannelFuture task_sendfile;
					// ctx.pipeline().get(io.netty.handler.ssl.SsslHandler.class) != null
					// => ctx.write(new HttpChunkedInput(new ChunkedFile(file, 0, filelen, 8192)),
					//              ctx.newProgressivePromise())
					// memory map:
					// MappedByteBuffer out = file.getChannel().map(FileChannel.MapMode.READ_WRITE, 0, length);
					task_sendfile = ctx.write(
							new DefaultFileRegion(file.getChannel(), 0, filelen),
							ctx.newProgressivePromise());
					task_sendfile.addListener(new ChannelProgressiveFutureListener() {

						@Override
						public void operationProgressed(
								ChannelProgressiveFuture future,
								long cur, long total) throws Exception {
							if (cur < 0) {
								System.out.println("preparing ...");
							} else {
								System.out.println(String.format("sending %d/%d ...", cur, total));
							}
						}

						@Override
						public void operationComplete(
								ChannelProgressiveFuture future) throws Exception {
							System.out.println("complete ...");
						}
						
					});
					return;
				} else {
					response = new DefaultFullHttpResponse(
							HttpVersion.HTTP_1_1,
							HttpResponseStatus.OK,
							Unpooled.wrappedBuffer(stream));
					response.headers()
						.set(HttpHeaders.Names.CONTENT_TYPE, "text/html")
						.set(HttpHeaders.Names.CONTENT_LENGTH, response.content().readableBytes());
					if (HttpHeaders.isKeepAlive(request)) {
						response.headers().set(HttpHeaders.Names.CONNECTION, HttpHeaders.Values.KEEP_ALIVE);
					}
				}
				ctx.writeAndFlush(response);
			}
		}
		
		@Override
		public void channelReadComplete(ChannelHandlerContext ctx)
				throws Exception {
			ctx.flush();
		}
		
		@Override
		public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
			System.err.println(cause.getMessage());
			ctx.close();
		}
	}

	public static void main(String[] args) throws Exception {
		System.out.println("Http server listening ...");
		new HttpServer().start("127.0.0.1", 8080);
	}

}
