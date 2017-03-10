package org.elasticsearch.plugin.example;

import org.elasticsearch.common.inject.Inject;
import org.elasticsearch.common.settings.Settings;
import org.elasticsearch.plugins.Plugin;

import org.elasticsearch.client.Client;
import org.elasticsearch.common.xcontent.XContentBuilder;
import org.elasticsearch.common.xcontent.json.JsonXContent;
import org.elasticsearch.rest.RestModule;
import org.elasticsearch.rest.BaseRestHandler;
import org.elasticsearch.rest.RestController;
import org.elasticsearch.rest.RestRequest;
import org.elasticsearch.rest.RestChannel;
import org.elasticsearch.rest.RestRequest.Method;
import org.elasticsearch.rest.RestStatus;
import org.elasticsearch.rest.BytesRestResponse;

import java.util.Map;
import org.elasticsearch.common.Nullable;
import org.elasticsearch.script.ScriptModule;
import org.elasticsearch.script.AbstractFloatSearchScript;
import org.elasticsearch.script.ExecutableScript;
import org.elasticsearch.script.NativeScriptFactory;
import org.elasticsearch.script.ScriptException;
import org.elasticsearch.search.lookup.IndexField;
import org.elasticsearch.search.lookup.IndexFieldTerm;

import org.elasticsearch.action.search.SearchResponse;
import org.elasticsearch.search.SearchHits;
import org.elasticsearch.search.SearchHit;

public class ExamplePlugin extends Plugin {

    @Inject public ExamplePlugin(Settings settings) {
    }

    @Override public String name() {
        return "example-plugin";
    }

    @Override public String description() {
        return "Example Plugin Description";
    }

    /////////////////////////////////////////////////////////

    /*
     * curl -XGET localhost:9200/_hello?who=world
     */

    public static class HelloRestHandler extends BaseRestHandler {
        @Inject
        public HelloRestHandler(Settings settings, Client client, RestController controller) {
            super(settings, controller, client);
            controller.registerHandler(Method.GET, "/_hello", this);
        }

        @Override
        public void handleRequest(RestRequest request, RestChannel channel, Client client) throws Exception {
            String who = request.param("who", null);
            if (who == null) who = "world";
            SearchResponse result = client.prepareSearch().execute().actionGet();
            SearchHits htis = result.getHits();
            long n = hits.totalHits();
            for (SearchHit one : hits.getHits()) {
                System.out.println(one.getId());
            }
            sendResponse(request, channel, who);
        }

        private void sendResponse(RestRequest request, RestChannel channel, String name) throws Exception {
            XContentBuilder builder = JsonXContent.contentBuilder();
            builder.startObject().field("say", "hello, " + name);
            builder.endObject();
            channel.sendResponse(new BytesRestResponse(RestStatus.OK, builder));
        }
    }

    public void onModule(RestModule module) {
        module.addRestAction(HelloRestHandler.class);
    }

    /////////////////////////////////////////////////////////

    /*
     * curl -XPOST localhost:9200/_search -d '{
     *   "query": {
     *     "function_score": {
     *       "query": {
     *            "match_all": {}
     *       },
     *       "functions": [{
     *         "script_score": {
     *           "script": "hello_score_script",
     *           "lang" : "native"
     *         }
     *       }]
     *     }
     *   }
     * }'
     */

    public static class HelloNativeScriptFactory implements NativeScriptFactory {
        @Override public ExecutableScript newScript(@Nullable Map<String, Object> params) {
            return new HelloNativeScript();
        }

        @Override public boolean needsScores() { return false; }
    }

    public static class HelloNativeScript extends AbstractFloatSearchScript {
        public MyNativeScript() {}
        public MyNativeScript(Map<String, Object> params) {}

        @Override
        public float runAsFloat() {
            // IndexField inf = this.indexLookup().get(field);
            // IndexFieldTerm itf = inf.get(term); => itf.df(), itf.tf()
            return (float) 5.0;
        }
    }

    public void onModule(ScriptModule module) {
        module.registerScript("hello_score_script", HelloNativeScriptFactory.class);
    }

}
