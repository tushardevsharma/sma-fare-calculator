namespace SMAFareCalculator.API;

public static class UserFacingMessages
{
    public const string BadRequestMessage =
        @"The input provided is in an incorrect format. Please provide the input in the following format: {tripFromLine},{tripToLine},{tripTime};{tripFromLine},{tripToLine},{tripTime};...(more trips). For e.g - ""Green,Green,2021-03-24T07:58:30;Green,Red,2021-03-24T09:58:30;Red,Red,2021-03-25T11:58:30;...(more trips)""";
}