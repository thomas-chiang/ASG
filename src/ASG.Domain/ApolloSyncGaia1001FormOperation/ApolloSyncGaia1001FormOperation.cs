using System.ComponentModel;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.Common;
using ASG.Domain.Gaia1001Forms;

namespace ASG.Domain.ApolloSyncGaia1001FormOperation;

public class ApolloSyncGaia1001FormOperation
{
    public required Gaia1001Form Gaia1001Form { get; set; }
    public required ApolloAttendance ApolloAttendance { get; set; }
    public List<AnonymousRequest> AnonymousRequests { get; set; } = new();

    public Sync1001FormSituation? Situation { get; set; }

    public ApolloAttendance? UpdatedApolloAttendance { get; set; }

    public void SetAnonymousRequestsToBeSent()
    {
        if (HasOtherEffectiveAttendanceMethod()) return;

        var apollo1001Form = GetApollo1001FormMatchingGaia1001Form();

        switch (Gaia1001Form.FormStatus)
        {
            case Gaia1001FormStatus.Approved when apollo1001Form == null:
                AppendRequestToCreateAndApproveForm();
                break;
            case Gaia1001FormStatus.Approved or Gaia1001FormStatus.Rejected when
                apollo1001Form is { ApprovalStatus: Apollo1001ApprovalStatus.Unknown }:
                AppendRequestToApproveForm();
                break;
            case Gaia1001FormStatus.Deleted when
                apollo1001Form is { ApprovalStatus: Apollo1001ApprovalStatus.Ok }:
                AppendRequestToRecallForm();
                break;
            default:
                AppendFailedRequests();
                break;
        }
    }

    public void SetSituation()
    {
        if (HasOtherEffectiveAttendanceMethod())
        {
            Situation = Gaia1001Form.AttendanceType switch
            {
                AttendanceType.ClockIn => Sync1001FormSituation.AlreadyHasClockInRecord,
                AttendanceType.ClockOut => Sync1001FormSituation.AlreadyHasClockOutRecord,
                _ => Sync1001FormSituation.AlreadyHasOtherAttendanceTypeRecordOrIsStillApproving
            };
            return;
        }

        if (UpdatedApolloAttendance == null)
            throw new InvalidOperationException("Need to fetch again apollo attendance.");

        if (UpdatedApolloAttendance.Apollo1001Forms.Any(form =>
                form.FormKind == Gaia1001Form.FormKind &&
                form.FormNo == Gaia1001Form.FormNo &&
                (
                    (
                        form.ApprovalStatus == Apollo1001ApprovalStatus.Ok
                        && Gaia1001Form.FormStatus == Gaia1001FormStatus.Approved
                    )
                    ||
                    (
                        form.ApprovalStatus == Apollo1001ApprovalStatus.Unknown
                        && new HashSet<Gaia1001FormStatus> {
                            Gaia1001FormStatus.WaitingApprove,
                            Gaia1001FormStatus.UnderApproving
                        }.Contains(Gaia1001Form.FormStatus)
                    )
                    ||
                    (
                        form.ApprovalStatus == Apollo1001ApprovalStatus.Deny
                        && Gaia1001Form.FormStatus == Gaia1001FormStatus.Rejected
                    )
                    ||
                    (
                        form.ApprovalStatus == Apollo1001ApprovalStatus.Delete &&
                        Gaia1001Form.FormStatus == Gaia1001FormStatus.Deleted
                    )
                )
            )) Situation = Sync1001FormSituation.NormalFailSync;
    }

    private bool HasOtherEffectiveAttendanceMethod()
    {
        // already has effective record with other AttendanceMethod
        return ApolloAttendance.ApolloAttendanceHistories.Any(history =>
                   history.IsEffective && history.AttendanceMethod != AttendanceMethod.Approval)
               || ( // already has other effective 1001 form
                   ApolloAttendance.ApolloAttendanceHistories.Any(history =>
                       history.IsEffective && history.AttendanceMethod == AttendanceMethod.Approval)
                   &&
                   ApolloAttendance.Apollo1001Forms.Any(form =>
                       form.ApprovalStatus == Apollo1001ApprovalStatus.Ok && form.FormNo != Gaia1001Form.FormNo)
               );
    }

    private Apollo1001Form? GetApollo1001FormMatchingGaia1001Form()
    {
        return ApolloAttendance.Apollo1001Forms.FirstOrDefault(form =>
            form.FormKind == Gaia1001Form.FormKind && form.FormNo == Gaia1001Form.FormNo);
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

    // public ApolloAttendanceHistory? GetApolloEffectiveHistory()
    // {
    //     var isEffectiveHistories = ApolloAttendance.ApolloAttendanceHistories.Where(h => h.IsEffective).ToList();
    //
    //     if (isEffectiveHistories.Count > 1)
    //         throw new InvalidOperationException("ApolloAttendanceHistories must only contain one effective item.");
    //
    //     return isEffectiveHistories.SingleOrDefault();
    // }
}