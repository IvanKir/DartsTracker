using Android.Support.V4.App;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace DartsTracker.Adapters
{
    class TabsFragmentAdapter : FragmentStatePagerAdapter
    {

        private string[] Titles;
        private string groupName;
        private string[] playersNames;

        public TabsFragmentAdapter(FragmentManager fm, string groupName, string[] titles, string[] playersNames)
            : base(fm)
        {
            Titles = titles;
            this.groupName = groupName;
            this.playersNames = playersNames;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {

            return new Java.Lang.String(Titles[position]);
        }

        public override int Count => Titles.Length;

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return new GroupFragment(groupName);
                case 1:
                    return new PlayerFragment(groupName, playersNames);
                case 2:
                default:
                    return new GroupFragment(groupName);
            }
        }
    }
}