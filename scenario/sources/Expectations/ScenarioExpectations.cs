using rharel.Debug;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.CouplesTherapyExample.Scene;
using rharel.M3PD.Expectations.Arrangement;
using static rharel.M3PD.Expectations.Arrangement.DialogueMoveNLI;

namespace rharel.M3PD.CouplesTherapyExample.Expectations
{
    /// <summary>
    /// Instantiates interaction expectation arrangements for the scenario.
    /// </summary>
    internal static class ScenarioExpectations
    {
        /// <summary>
        /// Creates a new expectation arrangement for the specified session.
        /// </summary>
        /// <param name="session">
        /// The session for which to make the arrangement.
        /// </param>
        /// <returns>
        /// A new expectation arrangement.
        /// </returns>
        public static Node Create(Session session)
        {
            Require.IsNotNull(session);

            therapist = session.Therapist.ID;
            patients = new Pair<string>(
                session.Patients.First.ID,
                session.Patients.Second.ID
            );

            expect = new NodeNLI();

            // ----- BEGIN EXPECTATION ARRANGEMENT ------ //

            var arrangement = expect.Sequence(
                "session",

                // Mutual exchange of greetings:
                expect.All("greetings", Greetings()),
                // Counseling begins:
                expect.Any(
                    "counseling",  // issue discussion or session ending

                    expect.Repeat(expect.OneOf(
                        "issue discussion with either patient",

                        IssueDiscussion(patients.First),
                        IssueDiscussion(patients.Second)
                    )),
                    expect.Event(therapist, Move<SessionClosing>())
                ),
                // Counseling has ended:
                expect.All(
                    "end of counseling",  // patients acknowledge end of session

                    expect.Event(patients.First, Move<Acknowledgement>()),
                    expect.Event(patients.Second, Move<Acknowledgement>())
                ),
                // Mutual exchange of goodbyes:
                expect.All("goodbyes", Goodbyes())
            );
            return arrangement;
        }

        private static Node[] Greetings()
        {
            return new Node[4]
            {
                expect.Event(therapist, Move<Greeting>(patients.First)),
                expect.Event(therapist, Move<Greeting>(patients.Second)),
                expect.Event(patients.First, Move<Greeting>(therapist)),
                expect.Event(patients.Second, Move<Greeting>(therapist)),
            };
        }
        private static Node[] Goodbyes()
        {
            return new Node[4]
            {
                expect.Event(therapist, Move<Goodbye>(patients.First)),
                expect.Event(therapist, Move<Goodbye>(patients.Second)),
                expect.Event(patients.First, Move<Goodbye>(therapist)),
                expect.Event(patients.Second, Move<Goodbye>(therapist)),
            };
        }

        private static Node IssueDiscussion(string patient)
        {
            // Here we will be expecting the patient to discuss a single issue 
            // and then refuse to discuss any more.

            bool there_are_undisclosed_issues = true;
            bool therapist_thinks_there_are_undisclosed_issues = true;

            Node issue_shared;
            Node issue_sharing_declined;

            var expectations = expect.If(() => therapist_thinks_there_are_undisclosed_issues,
                expect.Sequence(
                    $"therapist invites {patient} to share an issue",

                    expect.Event(therapist, Move<IssueSharingInvitation>(patient)), 
                    expect.OneOf(
                        $"{patient} either accepts or declines",

                        expect.If(() => there_are_undisclosed_issues,
                            issue_shared = expect.Sequence(
                                $"{patient} shares an issue and the therapist comments",

                                expect.Event(patient, Move<Acknowledgement>()),
                                expect.Event(patient, Move<IssueSharing>()),
                                expect.Event(therapist, Move<Acknowledgement>()),
                                expect.Event(therapist, Move<AdviceDispensation>()),
                                expect.Event(patient, Move<Acknowledgement>())
                            )
                        ),
                        issue_sharing_declined = expect.Sequence(
                            $"{patient} declines to share and the therapist acknowledges",

                            expect.Event(patient, Move<IssueSharingDeclination>()),
                            expect.Event(therapist, Move<Acknowledgement>())
                        )
                    )
                )
            );

            // Hook up some logic to when the following events transpire:
            issue_shared.Satisfied += (_) => there_are_undisclosed_issues = false;
            issue_sharing_declined.Satisfied += (_) => therapist_thinks_there_are_undisclosed_issues = false;

            return expectations;
        }

        private static NodeNLI expect;
        private static string therapist;
        private static Pair<string> patients;
    }
}
