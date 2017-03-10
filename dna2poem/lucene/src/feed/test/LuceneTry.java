package feed.test;

import org.apache.lucene.analysis.Analyzer;
import org.apache.lucene.analysis.standard.StandardAnalyzer;
import org.apache.lucene.document.Document;
import org.apache.lucene.document.Field;
import org.apache.lucene.document.StringField;
import org.apache.lucene.document.TextField;
import org.apache.lucene.index.DirectoryReader;
import org.apache.lucene.index.IndexReader;
import org.apache.lucene.index.IndexWriter;
import org.apache.lucene.index.IndexWriterConfig;
import org.apache.lucene.queryparser.classic.QueryParser;
import org.apache.lucene.search.IndexSearcher;
import org.apache.lucene.search.Query;
import org.apache.lucene.search.ScoreDoc;
import org.apache.lucene.search.TopScoreDocCollector;
import org.apache.lucene.store.Directory;
import org.apache.lucene.store.RAMDirectory;

public class LuceneTry {

	public static void addDoc(IndexWriter w, String name, String contents) {
		Document doc = new Document();
		doc.add(new StringField(   "title",     name, Field.Store.YES));
		doc.add(new   TextField("contents", contents, Field.Store.YES));
		try {
			w.addDocument(doc);
		} catch (Exception e) {}
	}
	
	public static void main(String[] args) throws Exception {
		Directory dir = new RAMDirectory();
		// import java.nio.file.Paths;
		// import org.apache.lucene.store.FSDirectory;
		// Directory dir = FSDirectory.open(Paths.get(""));
		Analyzer analyzer = new StandardAnalyzer();
		IndexWriterConfig iwc = new IndexWriterConfig(analyzer);
		IndexWriter writer = new IndexWriter(dir, iwc);
		addDoc(writer, "When You Are Old",
				"Murmur, a little sadly, how Love fled " +
				"And paced upon the mountains overhead " +
				"And hid his face amid a crowd of stars. ");
		addDoc(writer, "The Blind Women",
				"And Death, who plucks eyes like flowers, " +
				"doesn't find my eyes.");
		addDoc(writer, "Your Song",
				"I hope you don't mind that I put down in words. " +
				"How wonderful life is while you're in the world.");
		writer.close();
		
		String q = "little";
		QueryParser parser = new QueryParser("contents", analyzer);
		Query query = parser.parse(q);
		IndexReader reader = DirectoryReader.open(dir);
		IndexSearcher isearch = new IndexSearcher(reader);
		TopScoreDocCollector collect = TopScoreDocCollector.create(100);
		isearch.search(query, collect);
		ScoreDoc[] hits = collect.topDocs().scoreDocs;
		for (int i = 0; i<hits.length; i++) {
			Document d = isearch.doc(hits[i].doc);
			System.out.println(String.format(
					"%.6f > %s : %s",
					hits[i].score, d.get("title"), d.get("contents")));
		}
		if (hits.length == 0) {
			System.out.println("-");
		}
		reader.close();
	}

}
