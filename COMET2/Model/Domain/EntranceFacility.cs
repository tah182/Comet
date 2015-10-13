using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class EntranceFacility {
        public EntranceFacility(
                        Building address,
                        string legacy,
                        string trailName,
                        int? rfaid,
                        string vendor,
                        string ecckt,
                        bool highSpeed,
                        int usedSlots,
                        int totalSlots,
                        int ranking,
                        bool aOrdering
            ) {
                this.Address = address;
                this.Legacy = legacy;
                this.TrailName = trailName;
                this.RFAID = rfaid;
                this.Vendor = vendor;
                this.ECCKT = ecckt;
                this.HighSpeed = highSpeed;
                this.UsedSlots = usedSlots;
                this.TotalSlots = totalSlots;
                this.Ranking = ranking;
                this.AOrdering = aOrdering;
        }

        public EntranceFacility(Building address, 
                                int lata, 
                                decimal custIndPerc, 
                                decimal custHybridIndPerc, 
                                decimal switchIndPerc,
                                decimal ipNetIndPerc, 
                                decimal netOtherIndPerc, 
                                decimal noProdIndPerc, 
                                int sdpTrailNum, 
                                int lsefTrailNum,
                                int hsefTrailNum, 
                                int nniTrailNum, 
                                decimal lsefUtil, 
                                decimal hsefUtil, 
                                decimal nniUtil,
                                string nodeName, 
                                string lifecycleStatus, 
                                string facilityType, 
                                string recordOwner, 
                                string primaryHomingGateway,
                                string ilecClli, 
                                decimal lsefTrendSlope, 
                                decimal hsefTrendSlope, 
                                decimal nniTrendSlope) 
                : this (address, lata, custIndPerc, custHybridIndPerc, switchIndPerc, ipNetIndPerc, netOtherIndPerc, noProdIndPerc,
                        sdpTrailNum, lsefTrailNum, hsefTrailNum, nniTrailNum, lsefUtil, hsefUtil, nniUtil, lsefTrendSlope, hsefTrendSlope, nniTrendSlope) {

            this.IlecColo = new IlecColo(nodeName, lifecycleStatus, facilityType, recordOwner, primaryHomingGateway, ilecClli);
        }

        public EntranceFacility(Building address, int lata, decimal custIndPerc, decimal custHybridIndPerc, decimal switchIndPerc,
                                decimal ipNetIndPerc, decimal netOtherIndPerc, decimal noProdIndPerc, int sdpTrailNum, int lsefTrailNum,
                                int hsefTrailNum, int nniTrailNum, decimal lsefUtil, decimal hsefUtil, decimal nniUtil,
                                decimal lsefTrendSlope, decimal hsefTrendSlope, decimal nniTrendSlope) {

            this.Address = address;
            this.Lata = lata;
            this.CustIndPerc = custIndPerc;
            this.CustHybridIndPerc = custHybridIndPerc;
            this.SwitchIndPerc = switchIndPerc;
            this.IpNetIndPerc = ipNetIndPerc;
            this.NetOtherIndPerc = netOtherIndPerc;
            this.NoProdIndPerc = noProdIndPerc;
            this.NumTrails = sdpTrailNum;
            this.NumLowSpeedShared = lsefTrailNum;
            this.NumHighSpeedShared = hsefTrailNum;
            this.NumNni = nniTrailNum;
            this.LowSpeedSharedUtilization = lsefUtil;
            this.HighSpeedSharedUtilization = hsefUtil;
            this.NniUtilization = nniUtil;
            this.HsefTrending = this.NumHighSpeedShared == 0 ?
                                TrendSlope.NONE : hsefTrendSlope == 0 ?
                                TrendSlope.FLAT : hsefTrendSlope > 0 ? TrendSlope.UP : TrendSlope.DOWN;
            this.LsefTrending = this.NumLowSpeedShared == 0 ?
                                TrendSlope.NONE : lsefTrendSlope == 0 ?
                                TrendSlope.FLAT : lsefTrendSlope > 0 ? TrendSlope.UP : TrendSlope.DOWN;
            this.NniTrending = this.NumNni == 0 ?
                                TrendSlope.NONE : nniTrendSlope == 0 ?
                                TrendSlope.FLAT : nniTrendSlope > 0 ? TrendSlope.UP : TrendSlope.DOWN;
        }

        public bool AOrdering {
            get;
            private set;
        }

        public string Legacy {
            get;
            private set;
        }

        public string TrailName {
            get;
            private set;
        }

        public int? RFAID {
            get;
            private set;
        }

        public string Vendor {
            get;
            private set;
        }

        public string ECCKT {
            get;
            private set;
        }

        public bool HighSpeed {
            get;
            private set;
        }

        public int UsedSlots {
            get;
            private set;
        }

        public int TotalSlots {
            get;
            private set;
        }

        public double Utilization {
            get {
                return TotalSlots == 0 ? 0 : UsedSlots * 1.0 / TotalSlots;
            }
        }

        public int Ranking {
            get;
            private set;
        }

        public Building Address {
            get;
            private set;
        }

        public IlecColo IlecColo {
            get;
            private set;
        }

        public int Lata {
            get;
            private set;
        }

        public TrendSlope HsefTrending {
            get;
            private set;
        }

        public TrendSlope LsefTrending {
            get;
            private set;
        }

        public TrendSlope NniTrending {
            get;
            private set;
        }

        public int NumTrails {
            get;
            private set;
        }

        public int NumHighSpeedShared {
            get;
            private set;
        }

        public int NumLowSpeedShared {
            get;
            private set;
        }

        public int NumNni {
            get;
            private set;
        }

        public bool hasHighSpeedShared {
            get { return this.NumHighSpeedShared > 0; }
        }

        public bool hasLowSpeedShared {
            get { return this.NumLowSpeedShared > 0; }
        }

        public bool hasNni {
            get { return this.NumNni > 0; }
        }

        public bool hasIlecColo {
            get { return this.IlecColo != null; }
        }

        public decimal HighSpeedSharedUtilization {
            get;
            private set;
        }

        public decimal LowSpeedSharedUtilization {
            get;
            private set;
        }

        public decimal NniUtilization {
            get;
            private set;
        }

        public decimal CustIndPerc {
            get;
            private set;
        }

        public decimal CustHybridIndPerc {
            get;
            private set;
        }

        public decimal SwitchIndPerc {
            get;
            private set;
        }

        public decimal IpNetIndPerc {
            get;
            private set;
        }

        public decimal NetOtherIndPerc {
            get;
            private set;
        }

        public decimal NoProdIndPerc {
            get;
            private set;
        }
    }
}