/**
 * @author Liu Jiayi
 * @date 2011.12.08
 */
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SQLite;

namespace PulseWaveAnalyzer.model
{
    public class DBConnector
    {
        private static string connstr = null;
        private SQLiteConnection conn;
        private SQLiteCommand cmd;

        public string filter(string _str, char[] _chars)
        {
            string tmp;
            foreach (char x in _chars)
            {
                tmp = new string(x, 1);
                _str = _str.Replace(tmp, "");
            }
            return _str;
        }

        public void open(string dbname)
        {
            if (dbname == null) return;
            if (connstr == null)
            {
                connstr = "Data Source=" + dbname;
            }
            if (conn != null)
            {
                if (conn.State == ConnectionState.Broken) conn.Close();
                if (conn.State != ConnectionState.Closed) return;
                conn.ConnectionString = "Data Source=" + dbname;
            }
            else
            {
                conn = new SQLiteConnection(connstr);
            }
            conn.Open();
        }
        public void close()
        {
            cmd = null;
            if (conn == null) return;
            if (conn.State == ConnectionState.Closed) return;
            conn.Close();
        }

        public void prepare(string _sql)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Open) return;
            cmd = new SQLiteCommand(_sql, conn);
        }
        public void setup(DbType _type, string key, object value)
        {
            if (cmd.Parameters.Contains(key)) return;
            cmd.Parameters.Add(key, _type);
            cmd.Parameters[key].Value = value;
        }

        public DataTable query()
        {
            if (conn == null || cmd == null) return null;
            if (conn.State != ConnectionState.Open) return null;
            DataTable r = null;
            SQLiteDataReader data = cmd.ExecuteReader();
            r = new DataTable();
            r.Load(data);
            data.Close();
            return r;
        }

        public object queryTop()
        {
            if (conn == null || cmd == null) return null;
            if (conn.State != ConnectionState.Open) return null;
            object r = null;
            r = cmd.ExecuteScalar();
            return r;
        }

        public int update()
        {
            if (conn == null || cmd == null) return 0;
            if (conn.State != ConnectionState.Open) return 0;
            int r = 0;
            r = cmd.ExecuteNonQuery();
            return r;
        }
    }
}
