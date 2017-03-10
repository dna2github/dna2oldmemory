package feed.test;

import java.util.ArrayList;

import io.netty.bootstrap.ServerBootstrap;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.Channel;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelFutureListener;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelOption;
import io.netty.channel.ChannelPipeline;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.SimpleChannelInboundHandler;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import io.netty.handler.codec.http.DefaultFullHttpResponse;
import io.netty.handler.codec.http.FullHttpRequest;
import io.netty.handler.codec.http.FullHttpResponse;
import io.netty.handler.codec.http.HttpHeaders;
import io.netty.handler.codec.http.HttpMethod;
import io.netty.handler.codec.http.HttpObjectAggregator;
import io.netty.handler.codec.http.HttpRequestDecoder;
import io.netty.handler.codec.http.HttpResponseEncoder;
import io.netty.handler.codec.http.HttpResponseStatus;
import io.netty.handler.codec.http.HttpVersion;
import io.netty.handler.codec.http.websocketx.CloseWebSocketFrame;
import io.netty.handler.codec.http.websocketx.PingWebSocketFrame;
import io.netty.handler.codec.http.websocketx.PongWebSocketFrame;
import io.netty.handler.codec.http.websocketx.TextWebSocketFrame;
import io.netty.handler.codec.http.websocketx.WebSocketFrame;
import io.netty.handler.codec.http.websocketx.WebSocketServerHandshaker;
import io.netty.handler.codec.http.websocketx.WebSocketServerHandshakerFactory;
import io.netty.handler.logging.LogLevel;
import io.netty.handler.logging.LoggingHandler;
import io.netty.handler.ssl.SslContext;
import io.netty.handler.ssl.SslContextBuilder;
import io.netty.handler.ssl.util.SelfSignedCertificate;
import io.netty.util.CharsetUtil;

public class WebsocketTry {

	private static final boolean SSL = System.getProperty("ssl") != null;
	private static final int PORT = Integer.parseInt(System.getProperty("port", SSL ? "8443" : "8080"));

	private static class IndexPage {
		private static final String NEWLINE = "\r\n";

		public static ByteBuf getContent(String webSocketLocation) {
			return Unpooled.copiedBuffer("<html><head><title>Web Socket Test</title></head>" + NEWLINE + "<body>"
					+ NEWLINE + "<script type=\"text/javascript\">" + NEWLINE + "var socket;" + NEWLINE
					+ "if (!window.WebSocket) {" + NEWLINE + "  window.WebSocket = window.MozWebSocket;" + NEWLINE + '}'
					+ NEWLINE + "if (window.WebSocket) {" + NEWLINE + "  socket = new WebSocket(\"" + webSocketLocation
					+ "\");" + NEWLINE + "  socket.onmessage = function(event) {" + NEWLINE
					+ "    var ta = document.getElementById('responseText');" + NEWLINE
					+ "    ta.value = ta.value + '\\n' + event.data" + NEWLINE + "  };" + NEWLINE
					+ "  socket.onopen = function(event) {" + NEWLINE
					+ "    var ta = document.getElementById('responseText');" + NEWLINE
					+ "    ta.value = \"Web Socket opened!\";" + NEWLINE + "  };" + NEWLINE
					+ "  socket.onclose = function(event) {" + NEWLINE
					+ "    var ta = document.getElementById('responseText');" + NEWLINE
					+ "    ta.value = ta.value + \"Web Socket closed\"; " + NEWLINE + "  };" + NEWLINE + "} else {"
					+ NEWLINE + "  alert(\"Your browser does not support Web Socket.\");" + NEWLINE + '}' + NEWLINE
					+ NEWLINE + "function send(message) {" + NEWLINE + "  if (!window.WebSocket) { return; }" + NEWLINE
					+ "  if (socket.readyState == WebSocket.OPEN) {" + NEWLINE + "    socket.send(message);" + NEWLINE
					+ "  } else {" + NEWLINE + "    alert(\"The socket is not open.\");" + NEWLINE + "  }" + NEWLINE
					+ '}' + NEWLINE + "</script>" + NEWLINE + "<form onsubmit=\"return false;\">" + NEWLINE
					+ "<input type=\"text\" name=\"message\" value=\"Hello, World!\"/>"
					+ "<input type=\"button\" value=\"Send Web Socket Data\"" + NEWLINE
					+ "       onclick=\"send(this.form.message.value)\" />" + NEWLINE + "<h3>Output</h3>" + NEWLINE
					+ "<textarea id=\"responseText\" style=\"width:500px;height:300px;\"></textarea>" + NEWLINE
					+ "</form>" + NEWLINE + "</body>" + NEWLINE + "</html>" + NEWLINE, CharsetUtil.US_ASCII);
		}

		private IndexPage() {
		}
	}

	private static class MyHttpServerHandler extends SimpleChannelInboundHandler<Object> {
		private static final String WEBSOCKET_PATH = "/websocket";

		private WebSocketServerHandshaker handshaker;
		private static ArrayList<Channel> websockets;
		private static Object wsLock;
		static {
			websockets = new ArrayList<>();
			wsLock = new Object();
		}

		@Override
		public void channelRead0(ChannelHandlerContext ctx, Object msg) {
			if (msg instanceof FullHttpRequest) {
				handleHttpRequest(ctx, (FullHttpRequest) msg);
			} else if (msg instanceof WebSocketFrame) {
				handleWebSocketFrame(ctx, (WebSocketFrame) msg);
			}
		}

		@Override
		public void channelReadComplete(ChannelHandlerContext ctx) {
			ctx.flush();
		}

		private void handleHttpRequest(ChannelHandlerContext ctx, FullHttpRequest req) {
			// Handle a bad request.
			if (!req.getDecoderResult().isSuccess()) {
				sendHttpResponse(ctx, req,
						new DefaultFullHttpResponse(HttpVersion.HTTP_1_1, HttpResponseStatus.BAD_REQUEST));
				return;
			}

			// Allow only GET methods.
			if (req.getMethod() != HttpMethod.GET) {
				sendHttpResponse(ctx, req,
						new DefaultFullHttpResponse(HttpVersion.HTTP_1_1, HttpResponseStatus.FORBIDDEN));
				return;
			}

			// Send the demo page and favicon.ico
			String uri = req.getUri();
			if ("/".equals(uri)) {
				ByteBuf content = IndexPage.getContent(getWebSocketLocation(req));
				FullHttpResponse res = new DefaultFullHttpResponse(HttpVersion.HTTP_1_1, HttpResponseStatus.OK,
						content);

				res.headers().set(HttpHeaders.Names.CONTENT_TYPE, "text/html; charset=UTF-8");
				res.headers().set(HttpHeaders.Names.CONTENT_LENGTH, res.content().readableBytes());

				sendHttpResponse(ctx, req, res);
				return;
			}
			if ("/favicon.ico".equals(uri)) {
				FullHttpResponse res = new DefaultFullHttpResponse(HttpVersion.HTTP_1_1, HttpResponseStatus.NOT_FOUND);
				sendHttpResponse(ctx, req, res);
				return;
			}
			
			if ("/dump".equals(uri)) {
				StringBuffer sbuf = new StringBuffer();
				for (Channel one : websockets) {
					if (one == null) continue;
					sbuf.append(one.toString());
					sbuf.append('\n');
				}
				ByteBuf content = Unpooled.copiedBuffer(new String(sbuf), CharsetUtil.UTF_8);
				FullHttpResponse res = new DefaultFullHttpResponse(
						HttpVersion.HTTP_1_1, HttpResponseStatus.OK, content);
				res.headers().set(HttpHeaders.Names.CONTENT_TYPE, "text/plain; charset=UTF-8");
				res.headers().set(HttpHeaders.Names.CONTENT_LENGTH, res.content().readableBytes());
				sendHttpResponse(ctx, req, res);
				return;
			}
			String[] split_uri = uri.split("/");
			if (split_uri.length > 2) {
				int index = Integer.parseInt(split_uri[1]);
				String message = split_uri[2].toUpperCase();
				Channel ws = websockets.get(index);
				if (!ws.isActive()) {
					sendHttpResponse(ctx, req, 
							new DefaultFullHttpResponse(HttpVersion.HTTP_1_1, HttpResponseStatus.NOT_FOUND));
					websockets.set(index, null);
					System.err.printf("%s disconnected", ws);
					return;
				}
				ws.writeAndFlush(new TextWebSocketFrame(message));
				System.err.printf("%s received_ex %s%n", ws, message);
				FullHttpResponse res = new DefaultFullHttpResponse(
						HttpVersion.HTTP_1_1, HttpResponseStatus.ACCEPTED);
				sendHttpResponse(ctx, req, res);
				return;
			}

			// Handshake
			WebSocketServerHandshakerFactory wsFactory = new WebSocketServerHandshakerFactory(getWebSocketLocation(req),
					null, true);
			handshaker = wsFactory.newHandshaker(req);
			if (handshaker == null) {
				WebSocketServerHandshakerFactory.sendUnsupportedVersionResponse(ctx.channel());
			} else {
				handshaker.handshake(ctx.channel(), req);
				synchronized (wsLock) {
					if (!websockets.contains(ctx.channel())) {
						websockets.add(ctx.channel());
					}
				}
			}
		}

		private void handleWebSocketFrame(ChannelHandlerContext ctx, WebSocketFrame frame) {

			// Check for closing frame
			if (frame instanceof CloseWebSocketFrame) {
				handshaker.close(ctx.channel(), (CloseWebSocketFrame) frame.retain());
				int index = websockets.indexOf(ctx);
				if (index >= 0) websockets.set(index, null);
				return;
			}
			if (frame instanceof PingWebSocketFrame) {
				ctx.channel().write(new PongWebSocketFrame(frame.content().retain()));
				return;
			}
			if (!(frame instanceof TextWebSocketFrame)) {
				throw new UnsupportedOperationException(
						String.format("%s frame types not supported", frame.getClass().getName()));
			}

			// Send the uppercase string back.
			String request = ((TextWebSocketFrame) frame).text();
			System.err.printf("%s received %s%n", ctx.channel(), request);
			ctx.channel().write(new TextWebSocketFrame(request.toUpperCase()));
		}

		private static void sendHttpResponse(ChannelHandlerContext ctx, FullHttpRequest req, FullHttpResponse res) {
			// Generate an error page if response getStatus code is not OK
			// (200).
			if (res.getStatus().code() != 200) {
				ByteBuf buf = Unpooled.copiedBuffer(res.getStatus().toString(), CharsetUtil.UTF_8);
				res.content().writeBytes(buf);
				buf.release();
				res.headers().set(HttpHeaders.Names.CONTENT_LENGTH, res.content().readableBytes());
			}

			// Send the response and close the connection if necessary.
			ChannelFuture f = ctx.channel().writeAndFlush(res);
			if (!HttpHeaders.isKeepAlive(req) || res.getStatus().code() != 200) {
				f.addListener(ChannelFutureListener.CLOSE);
			}
		}

		@Override
		public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
			cause.printStackTrace();
			ctx.close();
		}

		private static String getWebSocketLocation(FullHttpRequest req) {
			String location = req.headers().get(HttpHeaders.Names.HOST) + WEBSOCKET_PATH;
			if (SSL) {
				return "wss://" + location;
			} else {
				return "ws://" + location;
			}
		}
	}

	private static class MyHttpServerInitializer extends ChannelInitializer<SocketChannel> {
		private final SslContext sslCtx;

		public MyHttpServerInitializer(SslContext sslCtx) {
			this.sslCtx = sslCtx;
		}

		@Override
		public void initChannel(SocketChannel ch) {
			ChannelPipeline p = ch.pipeline();
			if (sslCtx != null) {
				p.addLast(sslCtx.newHandler(ch.alloc()));
			}
			p.addLast(new HttpRequestDecoder());
			p.addLast(new HttpResponseEncoder());
			p.addLast(new HttpObjectAggregator(65536));
			p.addLast(new MyHttpServerHandler());
		}

	}

	public static void main(String[] args) throws Exception {
		// Configure SSL.
		final SslContext sslCtx;
		if (SSL) {
			SelfSignedCertificate ssc = new SelfSignedCertificate();
			sslCtx = SslContextBuilder.forServer(ssc.certificate(), ssc.privateKey()).build();
		} else {
			sslCtx = null;
		}

		// Configure the server.
		EventLoopGroup bossGroup = new NioEventLoopGroup(1);
		EventLoopGroup workerGroup = new NioEventLoopGroup();
		try {
			ServerBootstrap b = new ServerBootstrap();
			b.option(ChannelOption.SO_BACKLOG, 1024);
			b.group(bossGroup, workerGroup).channel(NioServerSocketChannel.class)
					.handler(new LoggingHandler(LogLevel.INFO)).childHandler(new MyHttpServerInitializer(sslCtx));

			Channel ch = b.bind(PORT).sync().channel();

			System.err.println(
					"Open your web browser and navigate to " + (SSL ? "https" : "http") + "://127.0.0.1:" + PORT + '/');

			ch.closeFuture().sync();
		} finally {
			bossGroup.shutdownGracefully();
			workerGroup.shutdownGracefully();
		}
	}

}
