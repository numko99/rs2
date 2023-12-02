namespace Iter.Desktop.Helper
{
    public class SectionChangedEventArgs : EventArgs
    {
        public Section Section { get; }

        public SectionChangedEventArgs(Section section)
        {
            Section = section;
        }
    }

    public enum Section
    {
        HomeSection,
        AgencySection,
        UsersSection,
        ReportsSection
    }
}
