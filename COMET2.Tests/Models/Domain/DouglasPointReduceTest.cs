using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using COMET.Model.Domain.Shape;
using System.Collections.Generic;
using COMET.Model.Domain;

namespace COMET.Tests.Models.Domain {
    [TestClass]
    public class DouglasPointReduceTest {
        private List<Line> lineList = new List<Line>();

        [TestInitialize()]
        public void setup() {
            List<LatLng> points = new List<LatLng>();
            points.Add(new LatLng((decimal)45.448350, (decimal)-73.798240));
            points.Add(new LatLng((decimal)45.448100, (decimal)-73.799290));
            points.Add(new LatLng((decimal)45.448050, (decimal)-73.799490));
            points.Add(new LatLng((decimal)45.448000, (decimal)-73.799690));
            points.Add(new LatLng((decimal)45.447950, (decimal)-73.799890));
            points.Add(new LatLng((decimal)45.447910, (decimal)-73.800090));
            points.Add(new LatLng((decimal)45.447860, (decimal)-73.800280));
            points.Add(new LatLng((decimal)45.447810, (decimal)-73.800470));
            points.Add(new LatLng((decimal)45.447750, (decimal)-73.80068));
            points.Add(new LatLng((decimal)45.447700, (decimal)-73.80086));
            points.Add(new LatLng((decimal)45.447650, (decimal)-73.801060));
            points.Add(new LatLng((decimal)45.447590, (decimal)-73.801260));
            points.Add(new LatLng((decimal)45.44753, (decimal)-73.801450));
            points.Add(new LatLng((decimal)45.447480, (decimal)-73.801650));
            points.Add(new LatLng((decimal)45.447420, (decimal)-73.801840));
            points.Add(new LatLng((decimal)45.447360, (decimal)-73.802040));
            points.Add(new LatLng((decimal)45.447310, (decimal)-73.802230));
            points.Add(new LatLng((decimal)45.447250, (decimal)-73.802420));
            points.Add(new LatLng((decimal)45.447190, (decimal)-73.802630));
            points.Add(new LatLng((decimal)45.447130, (decimal)-73.802820));
            points.Add(new LatLng((decimal)45.44708, (decimal)-73.80301));
            points.Add(new LatLng((decimal)45.447020, (decimal)-73.803210));
            points.Add(new LatLng((decimal)45.446960, (decimal)-73.803400));
            points.Add(new LatLng((decimal)45.446900, (decimal)-73.803600));
            points.Add(new LatLng((decimal)45.446840, (decimal)-73.803780));
            points.Add(new LatLng((decimal)45.446790, (decimal)-73.803970));
            points.Add(new LatLng((decimal)45.446730, (decimal)-73.804160));
            points.Add(new LatLng((decimal)45.446670, (decimal)-73.804350));
            points.Add(new LatLng((decimal)45.446610, (decimal)-73.804540));
            points.Add(new LatLng((decimal)45.446550, (decimal)-73.804740));
            points.Add(new LatLng((decimal)45.446490, (decimal)-73.804930));
            points.Add(new LatLng((decimal)45.446430, (decimal)-73.805140));
            points.Add(new LatLng((decimal)45.446370, (decimal)-73.805330));
            points.Add(new LatLng((decimal)45.446310, (decimal)-73.805520));
            points.Add(new LatLng((decimal)45.446250, (decimal)-73.805720));
            points.Add(new LatLng((decimal)45.446200, (decimal)-73.805900));
            points.Add(new LatLng((decimal)45.446140, (decimal)-73.806090));
            points.Add(new LatLng((decimal)45.446070, (decimal)-73.806280));
            points.Add(new LatLng((decimal)45.446010, (decimal)-73.806470));
            points.Add(new LatLng((decimal)45.445940, (decimal)-73.806670));
            points.Add(new LatLng((decimal)45.445880, (decimal)-73.806870));
            points.Add(new LatLng((decimal)45.445820, (decimal)-73.807060));
            points.Add(new LatLng((decimal)45.445760, (decimal)-73.807260));
            points.Add(new LatLng((decimal)45.445710, (decimal)-73.807450));
            points.Add(new LatLng((decimal)45.445650, (decimal)-73.807650));
            points.Add(new LatLng((decimal)45.445590, (decimal)-73.807840));
            points.Add(new LatLng((decimal)45.445530, (decimal)-73.808030));
            points.Add(new LatLng((decimal)45.44547, (decimal)-73.808230));
            points.Add(new LatLng((decimal)45.445420, (decimal)-73.808420));
            points.Add(new LatLng((decimal)45.445360, (decimal)-73.808610));
            points.Add(new LatLng((decimal)45.445300, (decimal)-73.808810));
            points.Add(new LatLng((decimal)45.445240, (decimal)-73.809000));
            points.Add(new LatLng((decimal)45.445180, (decimal)-73.809200));
            points.Add(new LatLng((decimal)45.445120, (decimal)-73.809390));
            points.Add(new LatLng((decimal)45.445070, (decimal)-73.80959));
            points.Add(new LatLng((decimal)45.445010, (decimal)-73.809780));
            points.Add(new LatLng((decimal)45.444960, (decimal)-73.809970));
            points.Add(new LatLng((decimal)45.444900, (decimal)-73.810160));
            points.Add(new LatLng((decimal)45.444840, (decimal)-73.810360));
            points.Add(new LatLng((decimal)45.444770, (decimal)-73.810550));
            points.Add(new LatLng((decimal)45.444710, (decimal)-73.810740));
            points.Add(new LatLng((decimal)45.444660, (decimal)-73.810930));
            points.Add(new LatLng((decimal)45.444600, (decimal)-73.811120));
            points.Add(new LatLng((decimal)45.444540, (decimal)-73.811320));
            points.Add(new LatLng((decimal)45.444480, (decimal)-73.811520));
            points.Add(new LatLng((decimal)45.444420, (decimal)-73.811710));
            points.Add(new LatLng((decimal)45.444360, (decimal)-73.811920));
            points.Add(new LatLng((decimal)45.444290, (decimal)-73.812120));
            points.Add(new LatLng((decimal)45.444230, (decimal)-73.812310));
            points.Add(new LatLng((decimal)45.444170, (decimal)-73.812520));
            points.Add(new LatLng((decimal)45.444110, (decimal)-73.812710));
            points.Add(new LatLng((decimal)45.444050, (decimal)-73.812910));
            points.Add(new LatLng((decimal)45.443990, (decimal)-73.813110));
            points.Add(new LatLng((decimal)45.443930, (decimal)-73.813300));
            points.Add(new LatLng((decimal)45.443870, (decimal)-73.813500));
            points.Add(new LatLng((decimal)45.443820, (decimal)-73.813690));
            points.Add(new LatLng((decimal)45.443760, (decimal)-73.813880));
            points.Add(new LatLng((decimal)45.443710, (decimal)-73.814080));
            points.Add(new LatLng((decimal)45.443650, (decimal)-73.814280));
            points.Add(new LatLng((decimal)45.443600, (decimal)-73.81447));
            points.Add(new LatLng((decimal)45.443540, (decimal)-73.814660));
            points.Add(new LatLng((decimal)45.443490, (decimal)-73.814860));
            points.Add(new LatLng((decimal)45.443430, (decimal)-73.815060));
            points.Add(new LatLng((decimal)45.443380, (decimal)-73.815260));
            points.Add(new LatLng((decimal)45.44332, (decimal)-73.815460));
            points.Add(new LatLng((decimal)45.443270, (decimal)-73.815670));
            points.Add(new LatLng((decimal)45.443220, (decimal)-73.815860));
            points.Add(new LatLng((decimal)45.443160, (decimal)-73.816050));
            points.Add(new LatLng((decimal)45.443110, (decimal)-73.81626));
            points.Add(new LatLng((decimal)45.443050, (decimal)-73.816450));
            points.Add(new LatLng((decimal)45.443000, (decimal)-73.816650));
            points.Add(new LatLng((decimal)45.442950, (decimal)-73.816860));
            points.Add(new LatLng((decimal)45.442890, (decimal)-73.817050));
            points.Add(new LatLng((decimal)45.442840, (decimal)-73.817240));
            points.Add(new LatLng((decimal)45.442790, (decimal)-73.817450));
            points.Add(new LatLng((decimal)45.442740, (decimal)-73.817640));
            points.Add(new LatLng((decimal)45.442690, (decimal)-73.817840));
            points.Add(new LatLng((decimal)45.442640, (decimal)-73.818040));
            points.Add(new LatLng((decimal)45.442580, (decimal)-73.818240));
            points.Add(new LatLng((decimal)45.442530, (decimal)-73.818450));
            points.Add(new LatLng((decimal)45.442470, (decimal)-73.818640));
            points.Add(new LatLng((decimal)45.442410, (decimal)-73.818840));
            points.Add(new LatLng((decimal)45.442350, (decimal)-73.819040));
            points.Add(new LatLng((decimal)45.442300, (decimal)-73.819240));
            points.Add(new LatLng((decimal)45.442250, (decimal)-73.819430));
            points.Add(new LatLng((decimal)45.442200, (decimal)-73.819630));
            points.Add(new LatLng((decimal)45.442140, (decimal)-73.819820));
            points.Add(new LatLng((decimal)45.442090, (decimal)-73.820030));
            points.Add(new LatLng((decimal)45.442040, (decimal)-73.820230));
            points.Add(new LatLng((decimal)45.441990, (decimal)-73.820430));
            points.Add(new LatLng((decimal)45.441930, (decimal)-73.820630));
            points.Add(new LatLng((decimal)45.441880, (decimal)-73.820820));

            this.lineList.Add(new Line("Test", points));
        }

        [TestMethod]
        public void TestMethod1() {
            int mapZoom = 5;
            
            System.Diagnostics.Debug.WriteLine("tolerance: " + tolerance(mapZoom));
            var a = this.lineList[0].Coordinates;
            DouglasPeuckerReduction dpr = new DouglasPeuckerReduction(a);
            List<LatLng> reducedLine = dpr.reduceLine(mapZoom);

            //System.Diagnostics.Debug.WriteLine(reducedLined[0].Coordinates);

            foreach (LatLng latlng in this.lineList[0].Coordinates) {
                if (!reducedLine.Contains(latlng))
                    System.Diagnostics.Debug.WriteLine("Removed: " + latlng.ToString());
            }
            foreach(LatLng latlng in reducedLine)
                System.Diagnostics.Debug.WriteLine("Kept: " + latlng.ToString());
        }

        [TestMethod]
        public void TestMethod2() {
            int mapZoom = 14;

            System.Diagnostics.Debug.WriteLine("tolerance: " + tolerance(mapZoom));
            var a = this.lineList[0].Coordinates;
            DouglasPeuckerReduction dpr = new DouglasPeuckerReduction(a);
            List<LatLng> reducedLine = dpr.reduceLine(mapZoom);

            //System.Diagnostics.Debug.WriteLine(reducedLined[0].Coordinates);

            foreach (LatLng latlng in this.lineList[0].Coordinates) {
                if (!reducedLine.Contains(latlng))
                    System.Diagnostics.Debug.WriteLine("Removed: " + latlng.ToString());
            }
            foreach (LatLng latlng in reducedLine)
                System.Diagnostics.Debug.WriteLine("Kept: " + latlng.ToString());
        }

        private double tolerance(int zoomLevel) {
            //return Math.Ceiling(zoomToScale()[zoomLevel] / zoomToScale()[14]);
            return Math.Pow(zoomLevel, zoomLevel / -2);
        }

        private IDictionary<int, double> zoomToScale() {
            IDictionary<int, double> scale = new Dictionary<int, double>();
            scale.Add(20, 1128.497220);
            scale.Add(19, 2256.994440);
            scale.Add(18, 4513.988880);
            scale.Add(17, 9027.977761);
            scale.Add(16, 18055.955520);
            scale.Add(15, 36111.911040);
            scale.Add(14, 72223.822090);
            scale.Add(13, 144447.644200);
            scale.Add(12, 288895.288400);
            scale.Add(11, 577790.576700);
            scale.Add(10, 1155581.153000);
            scale.Add(9, 2311162.307000);
            scale.Add(8, 4622324.614000);
            scale.Add(7, 9244649.227000);
            scale.Add(6, 18489298.450000);
            scale.Add(5, 36978596.910000);
            scale.Add(4, 73957193.820000);
            scale.Add(3, 147914387.600000);
            scale.Add(2, 295828775.300000);
            scale.Add(1, 591657550.500000);
            return scale;
        }
    }
}
