namespace ASG.Domain.ApolloAttendances;

public enum Apollo1001ApprovalStatus
{
    // 簽核中
    Unknown,
    
    // 同意
    Ok,
    
    // 不同意
    Deny,
    
    // 完成
    Complete,
    
    // 刪除作廢
    Delete
}