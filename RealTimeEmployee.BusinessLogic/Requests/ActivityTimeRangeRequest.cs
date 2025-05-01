namespace RealTimeEmployee.BusinessLogic.Requests;

public record ActivityTimeRangeRequest(
    DateTime StartDate,
    DateTime EndDate);
