using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MyWpfApp
{
    public static class AnimationHelper
    {
        public static ThicknessAnimation CreateThicknessAnimation(string propertyName, MoveDirection moveDirection, double offset, double duration, double decelerationRatio = 0, double beginTime = 0)
        {
            var a = GetThickness(moveDirection, offset);
            a.Duration = new Duration(TimeSpan.FromSeconds(duration));
            a.DecelerationRatio = decelerationRatio;
            a.BeginTime = TimeSpan.FromSeconds(beginTime);
            Storyboard.SetTargetProperty(a, new PropertyPath(propertyName));
            return a;
        }

        public static DoubleAnimation CreateDoubleAnimation(string propertyName, FadeType fadeType, double duration, double decelerateRatio = 0, double beginTime = 0)
        {
            var a = GetDoubleAnimation(fadeType);
            a.Duration = new Duration(TimeSpan.FromSeconds(duration));
            a.DecelerationRatio = decelerateRatio;
            a.BeginTime = TimeSpan.FromSeconds(beginTime);
            Storyboard.SetTargetProperty(a, new PropertyPath(propertyName));
            return a;
        }
        public static ObjectAnimationUsingKeyFrames CreateVisibilityAnimation(FadeType fadeType, double duration, double beginTime)
        {
            var a = new ObjectAnimationUsingKeyFrames { BeginTime = TimeSpan.FromSeconds(beginTime) };
            var f = GetVisibilityFrame(fadeType);
            f.KeyTime = TimeSpan.FromSeconds(duration);
            a.KeyFrames.Add(f);
            Storyboard.SetTargetProperty(a, new PropertyPath("Visibility"));
            return a;
        }

        private static ThicknessAnimation GetThickness(MoveDirection direction, double offset)
        {
            return direction switch
            {
                MoveDirection.FromLeft => new ThicknessAnimation { From = new Thickness(offset, 0, -offset, 0), To = new Thickness(0), },
                MoveDirection.FromRight => new ThicknessAnimation { From = new Thickness(-offset, 0, offset, 0), To = new Thickness(0), },
                MoveDirection.ToRight => new ThicknessAnimation { From = new Thickness(0), To = new Thickness(-offset, 0, offset, 0), },
                MoveDirection.ToLeft => new ThicknessAnimation { From = new Thickness(0), To = new Thickness(offset, 0, -offset, 0), },
                _ => new ThicknessAnimation(),
            };
        }

        private static DoubleAnimation GetDoubleAnimation(FadeType fadeType)
        {
            return fadeType switch
            {
                FadeType.FadeIn => new DoubleAnimation { From = 0, To = 1 },
                FadeType.FadeOut => new DoubleAnimation { From = 1, To = 0 },
                _ => new DoubleAnimation()
            };
        }

        private static DiscreteObjectKeyFrame GetVisibilityFrame(FadeType fadeType)
        {
            return fadeType switch
            {
                FadeType.FadeIn => new DiscreteObjectKeyFrame { Value = Visibility.Visible },
                FadeType.FadeOut => new DiscreteObjectKeyFrame { Value = Visibility.Collapsed },
                _ => new DiscreteObjectKeyFrame()
            };
        }
    }

    public enum MoveDirection
    {
        FromLeft = 1,
        ToRight,
        FromRight,
        ToLeft,
    }

    public enum FadeType
    {
        FadeIn = 1,
        FadeOut
    }

}
