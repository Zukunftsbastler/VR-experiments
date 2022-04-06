namespace VR_Experiment.Core
{
    /// <summary>
    /// The Roles for all Users.
    /// </summary>
    public enum Role : byte
    {
        //TODO: Discuss Godmode and its implementation.
        None = 0,
        Attendee = 1,
        Moderator = 2,
        Experimenter = 3,
    }

    public enum UpdateType
    {
        UpdateAndBeforRender,
        Update,
        BeforRender
    }

    public enum Hand : byte
    {
        None = 0,
        Left = 1,
        Right = 2,
    }
}
