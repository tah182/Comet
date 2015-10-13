using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.Shape;

namespace COMET.Model.Domain.ArcGis {
    public abstract class ABoundary : IBoundary {
        /// <summary>
        /// Constructor requiring id and objectId
        /// </summary>
        /// <param name="id">The external ID of the object.</param>
        /// <param name="objectId">The internal ID known to GIS database.</param>
        public ABoundary(long id, int objectId) {
            this.ID = id;
            this.ObjectID = objectId;
            this.Polygon = new List<ICoordShape>();
        }

        /// <summary>
        /// Constructor of the object using the name.
        /// </summary>
        /// <param name="name">The string name of the object</param>
        public ABoundary(string name) {
            this.Name = name;
        }

        public long ID {
            get;
            private set;
        }

        public IList<ICoordShape> Polygon {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public void addShape(List<LatLng> latLngs) {
            this.Polygon.Add(new Polygon(this.Name, latLngs));
        }

        public int ObjectID {
            get;
            private set;
        }
    }
}