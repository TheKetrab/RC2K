using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.Views.Components;


public class GetDriverPassCodeDialog : TextBoxDialog
{
    public GetDriverPassCodeDialog() : base(
        "Put driver pass code",
        "There is already existing driver (not a registered user) with this nick name. You own were given driver-pass-key when driver was created. Pass it here. In case of any problems contact TheKetrab via discord.",
        "Driver pass code",
        yes: "Ok")
    {

    }
}
