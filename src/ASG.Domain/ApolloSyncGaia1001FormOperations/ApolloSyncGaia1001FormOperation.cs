using System.ComponentModel;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Enums;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Events;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Requests;
using ASG.Domain.Common;
using ASG.Domain.Common.Enums;
using ASG.Domain.Gaia1001Forms;
using ASG.Domain.Gaia1001Forms.Enums;
using ASG.Domain.Gaia1001Forms.RequestBodys;
using ErrorOr;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations;

public class ApolloSyncGaia1001FormOperation : Entity
{
    public required Gaia1001Form Gaia1001Form { get; set; }
    public required ApolloAttendance ApolloAttendance { get; set; }
    public List<AnonymousRequest> AnonymousRequests { get; set; } = new();

    public Sync1001FormSituation? Situation { get; set; }

    public ApolloAttendance? UpdatedApolloAttendance { get; set; }

    public static bool HasOtherEffectiveAttendanceMethod(
        List<ApolloAttendanceHistory> apolloAttendanceHistories,
        List<Apollo1001Form> apollo1001Forms,
        int currentFormNo,
        Gaia1001FormStatus gaia1001FormStatus)
    {
        if (gaia1001FormStatus == Gaia1001FormStatus.Recall) return false;

        // Already has an effective record with a method other than Approval
        if (apolloAttendanceHistories.Any(history =>
                history.IsEffective && history.AttendanceMethod != AttendanceMethod.Approval))
            return true;

        // Already has other effective 1001 form with Approval attendance method
        if (apolloAttendanceHistories.Any(history =>
                history.IsEffective && history.AttendanceMethod == AttendanceMethod.Approval) &&
            apollo1001Forms.Any(form =>
                form.ApprovalStatus == Apollo1001ApprovalStatus.Ok && form.FormNo != currentFormNo))
            return true;

        return false;
    }

    public void SetSituationWithExistedEffectiveAttendance()
    {
        Situation = Gaia1001Form.AttendanceType switch
        {
            AttendanceType.ClockIn => Sync1001FormSituation.AlreadyHasClockInRecord,
            AttendanceType.ClockOut => Sync1001FormSituation.AlreadyHasClockOutRecord,
            _ => Sync1001FormSituation.AlreadyHasOtherAttendanceTypeRecordOrIsStillApproving
        };
    }

    public static Apollo1001Form? GetApollo1001FormMatchingGaia1001Form(List<Apollo1001Form>? apollo1001Forms,
        string formKind, int formNo)
    {
        return apollo1001Forms?.FirstOrDefault(form =>
            form.FormKind == formKind && form.FormNo == formNo);
    }

    public void SetAnonymousRequestsToBeSent(Apollo1001Form? apollo1001FormMatchingGaiaFormNo)
    {
        switch (Gaia1001Form.FormStatus)
        {
            case Gaia1001FormStatus.Approved when apollo1001FormMatchingGaiaFormNo == null:
                AppendRequestToCreateAndApproveForm();
                break;
            case Gaia1001FormStatus.Approved or Gaia1001FormStatus.Rejected when
                apollo1001FormMatchingGaiaFormNo is { ApprovalStatus: Apollo1001ApprovalStatus.Unknown }:
                AppendRequestToApproveForm();
                break;
            case Gaia1001FormStatus.Deleted when
                apollo1001FormMatchingGaiaFormNo is { ApprovalStatus: Apollo1001ApprovalStatus.Ok }:
                AppendRequestToRecallForm();
                break;
            default:
                AppendFailedRequests();
                break;
        }
    }

    public void SendAnonymousRequests()
    {
        DomainEvents.Add(new AnonymousRequestsSentEvent(AnonymousRequests));
    }

    public ErrorOr<Success> SetSituationAfterSendingAnonymousRequests(Apollo1001Form? updatedApollo1001Form)
    {
        if (updatedApollo1001Form == null)
            return ApolloSyncGaia1001FormOperationErrors.FailedApolloSyncGaia1001FormOperation;

        if (!IsNormalFailedSync(updatedApollo1001Form))
            return ApolloSyncGaia1001FormOperationErrors.FailedApolloSyncGaia1001FormOperation;
        Situation = Sync1001FormSituation.NormalFailedSync;
        return Result.Success;
    }

    private bool IsNormalFailedSync(Apollo1001Form updatedApollo1001Form)
    {
        return (updatedApollo1001Form.ApprovalStatus == Apollo1001ApprovalStatus.Ok &&
                Gaia1001Form.FormStatus == Gaia1001FormStatus.Approved) ||
               (updatedApollo1001Form.ApprovalStatus == Apollo1001ApprovalStatus.Unknown &&
                new HashSet<Gaia1001FormStatus>
                {
                    Gaia1001FormStatus.WaitingApprove,
                    Gaia1001FormStatus.UnderApproving
                }.Contains(Gaia1001Form.FormStatus)) ||
               (updatedApollo1001Form.ApprovalStatus == Apollo1001ApprovalStatus.Deny &&
                Gaia1001Form.FormStatus == Gaia1001FormStatus.Rejected) ||
               (updatedApollo1001Form.ApprovalStatus == Apollo1001ApprovalStatus.Delete &&
                Gaia1001Form.FormStatus == Gaia1001FormStatus.Deleted);
    }

    private void AppendRequestToCreateAndApproveForm()
    {
        AnonymousRequests.Add(new ApplyReCheckInFormRequest
        {
            RequestBody = new ApplyReCheckInFormRequestBody(
                Gaia1001Form.IsBehalf,
                Gaia1001Form.FormNo,
                Gaia1001Form.FormKind,
                Gaia1001Form.CompanyId.ToString(),
                Gaia1001Form.UserEmployeeId.ToString(),
                Gaia1001Form.UserEmployeeId.ToString(),
                (int)Gaia1001Form.AttendanceType,
                Gaia1001Form.AttendanceOn.AddHours(-8).ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                Gaia1001Form.PunchesLocationId.ToString(),
                Gaia1001Form.LocationDetails,
                Gaia1001Form.ReasonsForMissedClocking,
                Gaia1001Form.ExtendWorkHourType,
                Gaia1001Form.CheckInTimeoutType,
                Gaia1001Form.CheckInPersonalReasonTypeId.ToString(),
                Gaia1001Form.CheckInPersonalReason,
                true
            )
        });
    }

    private void AppendRequestToApproveForm()
    {
        AnonymousRequests.Add(new ApproveReCheckInFormRequest
        {
            RequestBody = new ApproveReCheckInFormRequestBody(
                Gaia1001Form.FormNo,
                Gaia1001Form.FormKind,
                GetGaia1001FormStatusDescription(Gaia1001Form.FormStatus),
                Gaia1001Form.CompanyId.ToString(),
                Gaia1001Form.UserEmployeeId.ToString(),
                Gaia1001Form.UserEmployeeId.ToString(),
                Gaia1001Form.FormStatus == Gaia1001FormStatus.Approved
            )
        });
    }

    private void AppendRequestToRecallForm()
    {
        AnonymousRequests.Add(new RecalledReCheckInFormRequest
        {
            RequestBody = new RecalledReCheckInFormRequestBody(
                Gaia1001Form.FormNo,
                Gaia1001Form.FormKind,
                Gaia1001Form.UserEmployeeId.ToString(),
                Gaia1001Form.CompanyId.ToString(),
                false
            )
        });
    }

    private void AppendFailedRequests()
    {
        if (Gaia1001Form.PtSyncFormOperations == null) return;

        var failedOperations = Gaia1001Form.PtSyncFormOperations
            .Where(op => op.Flag == Flag.Fail && op.RetryCount == 5)
            .ToList();

        foreach (var operation in failedOperations)
            switch (operation.FormAction)
            {
                case FormAction.Apply:
                    AnonymousRequests.Add(new ApplyReCheckInFormRequest
                    {
                        RequestBody = operation.FormActionJson
                    });
                    break;
                case FormAction.Approve:
                    AnonymousRequests.Add(new ApproveReCheckInFormRequest
                    {
                        RequestBody = operation.FormActionJson
                    });
                    break;
                case FormAction.Recalled:
                    AnonymousRequests.Add(new RecalledReCheckInFormRequest
                    {
                        RequestBody = operation.FormActionJson
                    });
                    break;
            }
    }

    private static string GetGaia1001FormStatusDescription(Gaia1001FormStatus status)
    {
        var field = status.GetType().GetField(status.ToString());
        if (field == null) return status.ToString();
        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? status.ToString() : attribute.Description;
    }
}