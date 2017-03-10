// http://stanfordnlp.github.io/CoreNLP/api.html

import java.util.List;
import java.util.Properties;

import edu.stanford.nlp.ling.CoreAnnotations.SentencesAnnotation;

import edu.stanford.nlp.pipeline.Annotation;
import edu.stanford.nlp.pipeline.StanfordCoreNLP;
import edu.stanford.nlp.semgraph.SemanticGraph;
import edu.stanford.nlp.semgraph.SemanticGraphCoreAnnotations.CollapsedCCProcessedDependenciesAnnotation;
import edu.stanford.nlp.util.CoreMap;

public class CoreNLP {
	public static void main(String[] args) {
		Properties props = new Properties();
                // download language model first: stanford-chinese-corenlp-<date>-models.jar
		try {
			props.load(Object.class.getResourceAsStream("/StanfordCoreNLP-chinese.properties"));
		} catch (Exception e) {
			System.exit(1);
		}
		props.put("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
		StanfordCoreNLP pipeline = new StanfordCoreNLP(props);
		String text = "世界依旧旋转，而我心中的一切却都一去不返，唯有静静眺望窗外，看时光流逝，四季变迁。";
		Annotation document = new Annotation(text);
		pipeline.annotate(document);
		List<CoreMap> sentences = document.get(SentencesAnnotation.class);
		for (CoreMap sentence : sentences) {
			SemanticGraph dependencies = sentence.get(CollapsedCCProcessedDependenciesAnnotation.class);
			dependencies.prettyPrint();
		}
	}
}
