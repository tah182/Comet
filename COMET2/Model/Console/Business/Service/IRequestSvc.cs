using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COMET.Server.Domain;
using COMET.Model.Console.Domain.View;
using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Business.Service {
    public interface IRequestSvc {
        IList<ALookup> getValueDrivers();

        IList<ALookup> getSupportUnits();

        IList<ALookup> getPrograms();
        
        IList<SupportArea> getSupportAreas();

        IList<LookupSorted> getElementStatus();

        IList<LookupSorted> getRequestStatus();

        IList<LookupSorted> getProjectStatus();

        IList<LookupActive> getRequestTypes();

        IList<LookupActive> getProjectTypes();
        
        IList<LookupActive> getRequestCategories();
                    
        NOTE saveNote(Note note);

        ELEMENT saveElement(ElementView element);

        ELEMENT updateElement(ElementView element);

        REQUEST saveRequest(RequestView request);
        
        REQUEST updateRequest(RequestView request);

        PROJECT saveProject(ProjectView project);

        PROJECT updateProject(ProjectView project);

        IList<PROJECT> getProjects();

        IList<REQUEST> getRequests();
                
        IList<ELEMENT> getElements();
        
        IList<NOTE> getNotes();

        int getProgramFromSupportArea(int supportAreaID);
    }
}
