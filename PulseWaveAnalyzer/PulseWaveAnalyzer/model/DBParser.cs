using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PulseWaveAnalyzer.model
{
    public static class DBParser
    {
        public const string version = "J.Y.Liu 2012";
        public const string path = ""; // @"C:\Users\J.Y.Liu\Desktop\thesis\AHTCM\";
        /*
         * medicine.bin
         * medicine table->id, name, mild, (ht,lr,ki,sp,lu,st,gb,bl,li,tb,si,pc)
         */
        public const string medicineDBName = path + "medicine.bin";
        /*
         * prescript.bin
         * prescript table->id, name, prescript
         * > prescript format: medicine id,weight,state;
         */
        public const string prescriptDBName = path + "prescript.bin";

        private static DBConnector db = new DBConnector();

        public static List<Medicine> medicineDB()
        {
            DataTable dt;
            db.open(medicineDBName);
            db.prepare("select * from [medicine] order by [id]");
            dt = db.query();
            db.close();

            if (dt == null) return null;
            if (dt.Rows == null) return null;
            List<Medicine> list = new List<Medicine>();
            foreach (DataRow x in dt.Rows) list.Add(convertToMedicine(x));
            list.Sort();
            return list;
        }
        private static Medicine convertToMedicine(DataRow dr)
        {
            if (dr == null) return null;
            Medicine m = new Medicine();
            m.id = int.Parse(dr["id"].ToString());
            m.name = dr["name"].ToString();
            m.mild = double.Parse(dr["mild"].ToString());
            m.merdian[0] = double.Parse(dr["ht"].ToString());
            m.merdian[1] = double.Parse(dr["lr"].ToString());
            m.merdian[2] = double.Parse(dr["ki"].ToString());
            m.merdian[3] = double.Parse(dr["sp"].ToString());
            m.merdian[4] = double.Parse(dr["lu"].ToString());
            m.merdian[5] = double.Parse(dr["st"].ToString());
            m.merdian[6] = double.Parse(dr["gb"].ToString());
            m.merdian[7] = double.Parse(dr["bl"].ToString());
            m.merdian[8] = double.Parse(dr["li"].ToString());
            m.merdian[9] = double.Parse(dr["tb"].ToString());
            m.merdian[10] = double.Parse(dr["si"].ToString());
            m.merdian[11] = double.Parse(dr["pc"].ToString());
            return m;
        }

        public static List<Prescript> prescriptDB(List<Medicine> medsdb)
        {
            if (medsdb == null) return null;
            DataTable dt;
            db.open(prescriptDBName);
            db.prepare("select * from [prescript]");
            dt = db.query();
            db.close();

            if (dt == null) return null;
            if (dt.Rows == null) return null;
            List<Prescript> list = new List<Prescript>();
            foreach (DataRow x in dt.Rows) list.Add(convertToPrescript(medsdb, x));
            list.Sort();
            return list;
        }
        private static Prescript convertToPrescript(List<Medicine> medsdb, DataRow dr)
        {
            if (dr == null) return null;
            Prescript p = new Prescript();
            p.id = int.Parse(dr["id"].ToString());
            p.name = dr["name"].ToString();
            string prescriptline = dr["prescript"].ToString();
            if (prescriptline == null) return null;
            string[] medicines = prescriptline.Split(";".ToCharArray());
            string[] para;
            int id; double weight, state;
            Medicine mmm = new Medicine();
            foreach (string x in medicines)
            {
                if (x.Length == 0) continue;
                para = x.Split(",".ToCharArray());
                id = int.Parse(para[0]);
                weight = double.Parse(para[1]);
                state = double.Parse(para[2]);
                mmm.id = id; id = medsdb.BinarySearch(mmm);
                if(id==-1) continue;
                p.add(medsdb[id], weight, state);
            }
            p.sort();
            return p;
        }

        public static void removeMedicine(int id)
        {
            db.open(medicineDBName);
            db.prepare("delete from [medicine] where [id]="+id);
            db.update();
            db.close();
        }
        public static void removePrescript(int id)
        {
            db.open(prescriptDBName);
            db.prepare("delete from [prescript] where [id]=" + id);
            db.update();
            db.close();
        }

        public static void addMedicine(List<Medicine> mlist, Medicine m)
        {
            int index = mlist.BinarySearch(m);
            if (index < 0)
            {
                db.open(medicineDBName);
                db.prepare("insert into [medicine] values (null,'"
                    + m.name + "'," + m.mild + ","
                    + m.merdian[0] + "," + m.merdian[1] + "," + m.merdian[2] + ","
                    + m.merdian[3] + "," + m.merdian[4] + "," + m.merdian[5] + ","
                    + m.merdian[6] + "," + m.merdian[7] + "," + m.merdian[8] + ","
                    + m.merdian[9] + "," + m.merdian[10] + "," + m.merdian[11] + ")");
                db.update();
                db.prepare("select last_insert_rowid()");
                DataTable dt = db.query();
                db.close();
                if (dt.Rows.Count > 0)
                {
                    int lastid = int.Parse(dt.Rows[0][0].ToString());
                    m.id = lastid;
                    //mlist.Add(m);
                }
            }
        }
        public static void addPrescript(List<Prescript> plist, Prescript p)
        {
            int index = plist.BinarySearch(p);
            if (index < 0)
            {
                db.open(prescriptDBName);
                db.prepare("insert into [prescript] values (null,'"
                    + p.name + "','" + p.toPrescriptString() + "')");
                db.update();
                db.prepare("select last_insert_rowid()");
                DataTable dt = db.query();
                db.close();
                if (dt.Rows.Count > 0)
                {
                    int lastid = int.Parse(dt.Rows[0][0].ToString());
                    p.id = lastid;
                    //plist.Add(p);
                }
            }
        }
        public static void updateMedicine(Medicine m)
        {
            db.open(medicineDBName);
            db.prepare("update [medicine] set [name]=@name,[mild]=@mild,[ht]=@ht,[lr]=@lr,[ki]=@ki,[sp]=@sp,[lu]=@lu,[st]=@st,[gb]=@gb,[bl]=@bl,[li]=@li,[tb]=@tb,[si]=@si,[pc]=@pc where [id]=@id");
            db.setup(DbType.String, "name", m.name);
            db.setup(DbType.Double, "mild", m.mild);
            db.setup(DbType.Double, "ht", m.merdian[0]);
            db.setup(DbType.Double, "lr", m.merdian[1]);
            db.setup(DbType.Double, "ki", m.merdian[2]);
            db.setup(DbType.Double, "sp", m.merdian[3]);
            db.setup(DbType.Double, "lu", m.merdian[4]);
            db.setup(DbType.Double, "st", m.merdian[5]);
            db.setup(DbType.Double, "gb", m.merdian[6]);
            db.setup(DbType.Double, "bl", m.merdian[7]);
            db.setup(DbType.Double, "li", m.merdian[8]);
            db.setup(DbType.Double, "tb", m.merdian[9]);
            db.setup(DbType.Double, "si", m.merdian[10]);
            db.setup(DbType.Double, "pc", m.merdian[11]);
            db.setup(DbType.Int32, "id", m.id);
            db.update();
            db.close();
        }
        public static void updatePrescript(Prescript p)
        {
            db.open(prescriptDBName);
            db.prepare("update [prescript] set [name]=@name,[prescript]=@script where [id]=@id");
            db.setup(DbType.String, "name", p.name);
            db.setup(DbType.String, "script", p.toPrescriptString());
            db.setup(DbType.Int32, "id", p.id);
            db.update();
            db.close();
        }

        public static List<Prescript> getRelatedPrescript(List<Prescript> plist, Medicine m)
        {
            List<Prescript> ps = new List<Prescript>();
            foreach (Prescript x in plist)
            {
                if (x.indexOf(m) >= 0) ps.Add(x);
            }
            return ps;
        }
    }
}
