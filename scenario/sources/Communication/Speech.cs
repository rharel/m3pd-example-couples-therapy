using rharel.Debug;
using rharel.M3PD.Common.Hashing;

namespace rharel.M3PD.CouplesTherapyExample.Communication
{
    /// <summary>
    /// Associates a string of text with a virtual human speaker.
    /// </summary>
    public struct Speech
    {
        /// <summary>
        /// Creates a new speech by the specified speaker and made up of the
        /// specified text.
        /// </summary>
        /// <param name="speaker_id">The speaker's identifier.</param>
        /// <param name="text">The speech text.</param>
        internal Speech(string speaker_id, string text)
        {
            Require.IsNotBlank(speaker_id);
            Require.IsNotBlank(text);

            SpeakerID = speaker_id;
            Text = text;
        }

        /// <summary>
        /// Gets the speaker's identifier.
        /// </summary>
        public string SpeakerID { get; }
        /// <summary>
        /// Gets the speech text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance. 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Speech other)
            {
                return other.SpeakerID.Equals(SpeakerID) &&
                       other.Text.Equals(Text);
            }
            return false;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = HashCombiner.Initialize();
            hash = HashCombiner.Hash(hash, SpeakerID);
            hash = HashCombiner.Hash(hash, Text);

            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Speech)}{{ " +
                   $"{nameof(SpeakerID)} = '{SpeakerID}', " +
                   $"{nameof(Text)} = '{Text}' }}";
        }
    }
}
