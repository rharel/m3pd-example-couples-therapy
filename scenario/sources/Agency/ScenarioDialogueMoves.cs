using rharel.Functional;

namespace rharel.M3PD.CouplesTherapyExample
{
    /// <summary>
    /// Indicates an object can be expressed as speech text.
    /// </summary>
    internal interface SpeechTextConvertible
    {
        /// <summary>
        /// Gets the speech text representation of this.
        /// </summary>
        /// <returns>Speech text string.</returns>
        string ToSpeechText(Optional<string> addressee);
    }

    /// <summary>
    /// A greeting from one agent to another.
    /// </summary>
    internal struct Greeting: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            $"Hello {addressee.Unwrap()}, it's nice to see you."
        );
    }
    /// <summary>
    /// A goodbye from one agent to another.
    /// </summary>
    internal struct Goodbye: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            $"Goodbye {addressee.Unwrap()}, until next time!"
        );
    }

    /// <summary>
    /// An invitation to share an issue.
    /// </summary>
    /// <remarks>
    /// This move's source is the therapist, and the addressee is a member of
    /// the patient couple.
    /// </remarks>
    internal struct IssueSharingInvitation: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            $"{addressee.Unwrap()}, would you like to share an issue that has been " +
            $"bothering you?"
        );
    }
    /// <summary>
    /// Sharing an issue a patient has with its partner
    /// </summary>
    /// <remarks>
    /// This is made in response to a <see cref="IssueSharingInvitation"/>.
    /// </remarks>
    internal struct IssueSharing: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            "I love my partner, but sometimes even little things can get " +
            "irritating. For example, often times it feels as if my partner " +
            "imitates what I say just to get on my nerves!"
        );
    }
    /// <summary>
    /// A declination to share an issue.
    /// </summary>
    /// <remarks>
    /// This is made in response to a <see cref="IssueSharingInvitation"/>.
    /// </remarks>
    internal struct IssueSharingDeclination: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            "No thanks, maybe some other time."
        );
    }


    /// <summary>
    /// Dispensation of advice by the therapist.
    /// </summary>
    internal struct AdviceDispensation: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            $"I think you should work on communication and share your " +
            $"feelings with each other more often."
        );
    }
    /// <summary>
    /// Declaring the end of a therapy session by the therapist.
    /// </summary>
    internal struct SessionClosing: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => (
            "Alright, we have achieved a lot today and made some good " +
            "progress, but I'm afraid our time is up."
        );
    }

    /// <summary>
    /// Generic acknowledgement.
    /// </summary>
    internal struct Acknowledgement: SpeechTextConvertible
    {
        public string ToSpeechText(Optional<string> addressee) => "Alright";
    }
}
