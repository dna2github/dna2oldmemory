using System;
using System.Collections.Generic;
using System.Text;
using PulseWaveAnalyzer.model;

namespace PulseWaveAnalyzer
{
    static class Database
    {
        public static List<Medicine> medicineDB = DBParser.medicineDB();
        public static List<Prescript> prescriptDB = DBParser.prescriptDB(medicineDB);
    }
}
