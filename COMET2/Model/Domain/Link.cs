
using System;
namespace COMET.Model.Domain {
    public class Link {
        private string relativeUrl;
        public Link(string text, string toolTip, string relativeUrl) {
            this.Text = text;
            this.ToolTip = toolTip;
            this.RelativeUrl = relativeUrl;
        }

        public string Text {
            get; 
            private set;
        }

        public string ToolTip {
            get;
            private set;
        }

        public string Class {
            get;
            set;
        }

        public string RelativeUrl {
            get {
                return this.relativeUrl;
            }
            private set {
                if (value.Length > 0) {
                    if (value.Substring(0, 1).Equals("/"))
                        this.relativeUrl = value.Substring(1);
                    this.relativeUrl = value;
                }
            }
        }

        public override bool Equals(object o) {
            if (o.GetType() != typeof (Link))
                return false;

            Link l = (Link) o;
            if (!this.Text.Equals(l.Text))
                return false;
            if (!this.ToolTip.Equals(l.ToolTip))
                return false;
            if (!this.RelativeUrl.Equals(l.RelativeUrl))
                return false;
            
            return true;
        }

        public override int GetHashCode() {
            byte[] hashByte = System.Text.Encoding.ASCII.GetBytes(this.RelativeUrl);
            return BitConverter.ToInt32(hashByte, 0).GetHashCode();
        }
    }
}