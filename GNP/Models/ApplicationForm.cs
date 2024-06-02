using System.ComponentModel.DataAnnotations.Schema;

namespace GNP.Models
{
    public enum ProtectiveEquipments
    {
        SafetyGlasses,
        Glooves,
        SafetyFootwares,
        HardHat,
        HiVisVest
    }
    public class ApplicationForm : BaseEntity<int>
    {
        [ForeignKey(nameof(Applicant))]
        public long ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        public string PermitNumber { get; set; }
        public string JSANumber { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string WorkActivityDescription { get; set; }
        public ReviewAssessment? ReviewAssessment { get; set; }
    }

    public class ReviewAssessment : BaseEntity<int>
    {
        [ForeignKey(nameof(ApplicationForm))]
        public int ApplicationFomrId { get; set; }
        public bool HasConductedRiskAssesment { get; set; }
        public bool HasIdentifiedPotentialHazard { get; set; }
        public bool EnsuredWorkActivity { get; set; }
        public bool VerifiedRequiredSkills { get; set; }
        public bool ConfirmedPermits { get; set; }
        public bool VerifiedWorkArea { get; set; }
        public bool HasEmergencyCommunication { get; set; }
        public bool HasSafetyCommunicated { get; set; }
        public bool VerifiedHandlingTools { get; set; }
        public bool IsTrainedInMachineOperations { get; set; }
        public bool HasPTWSystem { get; set; }
        public bool HasGeneralSafetyMeasures { get; set; }
        public bool HasEmergencyResponsePlan { get; set; }
        public List<ProtectiveEquipments>? ProtectiveEquipments { get; set; }
        public string? OtherProtectiveEquipment { get; set; }
        public bool AcceptDeclaration { get; set; }
        public string? AdminApprovalName { get; set; }
    }


}
