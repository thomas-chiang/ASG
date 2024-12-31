using ASG.Domain.ApolloAttendances;
using ASG.Domain.Gaia1001Forms;

namespace ASG.Domain.ApolloSyncGaia1001FormOperation;

public class ApolloSyncGaia1001FormOperation
{
    public required Gaia1001Form Gaia1001Form { get; set; }

    public required ApolloAttendance ApolloAttendance { get; set; }
    public List<AnonymousRequest> AnonymousRequests { get; set; } = new();

    public bool IsNeedOperation()
    {
        // Apollo already has record of other AttendanceMethod
        return ApolloAttendance.ApolloAttendanceHistories.Any(history =>
            history.IsEffective && history.AttendanceMethod != AttendanceMethod.Approval);
        // ||
        // // Gaia has canceled and Apollo does not have any effective record
        // (
        //     new HashSet<Gaia1001FormStatus>
        //     {
        //         Gaia1001FormStatus.Rejected,
        //         Gaia1001FormStatus.Recall,
        //         Gaia1001FormStatus.Deleted
        //     }.Contains(Gaia1001Form.FormStatus) && ApolloAttendance.ApolloAttendanceHistories
        //         .All(history => !history.IsEffective)
        // )
    }

    public ApolloAttendanceHistory? GetIsEffectiveHistory()
    {
        var isEffectiveHistories = ApolloAttendance.ApolloAttendanceHistories.Where(h => h.IsEffective).ToList();

        if (isEffectiveHistories.Count > 1)
            throw new InvalidOperationException("ApolloAttendanceHistories must only contain one effective item.");

        return isEffectiveHistories.SingleOrDefault();
    }

    public Apollo1001Form? GetApollo1001FormMatchingGaia1001Form()
    {
        return ApolloAttendance.Apollo1001Forms.FirstOrDefault(form =>
            form.FormKind == Gaia1001Form.FormKind && form.FormNo == Gaia1001Form.FormNo);
    }

    public void UpdateAnonymousRequests()
    {
        var apollo1001Form = GetApollo1001FormMatchingGaia1001Form();

        if (Gaia1001Form.FormStatus == Gaia1001FormStatus.Approved && apollo1001Form == null)
        {
            AppendApplyRequestToDirectlyApprovedStatus();
        }
    }

    private void AppendApplyRequestToDirectlyApprovedStatus()
    {
        var applyReCheckInFormOperation = Gaia1001Form.PtSyncFormOperations
            .FirstOrDefault(operation => operation.FormAction == FormAction.Apply);

        if (applyReCheckInFormOperation == null)
            throw new InvalidOperationException("Need to create a requestBody itself");

        if (applyReCheckInFormOperation.ApplyReCheckInFormRequestBody is ApplyReCheckInFormRequestBody requestBody)
        {
            // Create a new request body with IsApproved set to true
            var updatedRequestBody = new ApplyReCheckInFormRequestBody(
                requestBody.IsBehalf,
                requestBody.SourceFormNo,
                requestBody.SourceFormKind,
                requestBody.CompanyId,
                requestBody.EmployeeId,
                requestBody.UserEmployeeId,
                requestBody.AttendanceType,
                requestBody.AttendanceOn,
                requestBody.PunchesLocationId,
                requestBody.LocationDetails,
                requestBody.ReasonsForMissedClocking,
                requestBody.ExtendWorkHourType,
                requestBody.CheckInTimeoutType,
                requestBody.CheckInPersonalReasonTypeId,
                requestBody.CheckInPersonalReason,
                true
            );

            AnonymousRequests.Add(new AnonymousRequest
            {
                RequestBody = updatedRequestBody
            });
        }
    }
}