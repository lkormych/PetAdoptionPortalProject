
using PAPData.Entities;

namespace PetAdoptionPortal.Models;

public class ApplicationFilterViewModel
{
    public List<AppliedForAdoption> Applications { get; set; } = new List<AppliedForAdoption>();

    public List<AdoptionStatus> ApplicationStatuses { get; set; } = new List<AdoptionStatus>();
    public List<AdoptionStatus> SelectedAdoptionStatuses { get; set; } = new List<AdoptionStatus>();

}