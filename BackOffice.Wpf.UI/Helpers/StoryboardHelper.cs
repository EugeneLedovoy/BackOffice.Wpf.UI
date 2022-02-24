using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MyWpfApp
{
    public static class StoryboardHelper
    {
        public static void RunNewStoryboard(this FrameworkElement target, IEnumerable<Timeline> animations)
        {
            var sb = new Storyboard { Children = new TimelineCollection(animations) };
            sb.Begin(target);
        }

        public static void RunFromLeftStoryboard(this FrameworkElement target, double offset)
        {
            target.RunNewStoryboard(new List<Timeline>
            {
                AnimationHelper.CreateThicknessAnimation("Margin", MoveDirection.FromLeft, offset, 1, 0.3),
                AnimationHelper.CreateVisibilityAnimation(FadeType.FadeIn, 0.01, 0.25),
                AnimationHelper.CreateDoubleAnimation("Opacity", FadeType.FadeIn, 1.3, beginTime: 0.25)
            });
        }

        public static void RunToRightStoryboard(this FrameworkElement target, double offset)
        {
            target.RunNewStoryboard(new List<Timeline>
            {
                AnimationHelper.CreateThicknessAnimation("Margin", MoveDirection.ToRight, offset, 0.5),
                AnimationHelper.CreateDoubleAnimation("Opacity", FadeType.FadeOut, 0.5)
            });
        }

        public static void RunFromRightStoryboard(this FrameworkElement target, double offset)
        {
            target.RunNewStoryboard(new List<Timeline>
            {
                AnimationHelper.CreateThicknessAnimation("Margin", MoveDirection.FromRight, offset, 0.5),
                AnimationHelper.CreateDoubleAnimation("Opacity", FadeType.FadeIn, 0.5)
            });
        }

        public static void RunToLeftStoryboard(this FrameworkElement target, double offset)
        {
            target.RunNewStoryboard(new List<Timeline>
            {
                AnimationHelper.CreateThicknessAnimation("Margin", MoveDirection.ToLeft, offset, 0.5),
                AnimationHelper.CreateDoubleAnimation("Opacity", FadeType.FadeOut, 0.5)
            });
        }

    }
}
