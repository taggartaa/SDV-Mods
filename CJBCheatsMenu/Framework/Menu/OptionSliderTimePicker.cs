using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionSliderTimePicker : OptionSlider
    {
        private const int HOURS_IN_DAY = 20; // 6AM -> 2AM
        private const int MINUTES_PER_TIME_CHANGE = 10;
        private const int TIME_CHANGES_PER_HOUR = 60 / MINUTES_PER_TIME_CHANGE;
        private const int MINIMUM_START_TIME = 600;
        private const int MAXIMUM_END_TIME = MINIMUM_START_TIME + HOURS_IN_DAY * 100;

        static int ConvertSliderValueToTime(int sliderValue)
        {
            int minutes = (sliderValue % TIME_CHANGES_PER_HOUR) * MINUTES_PER_TIME_CHANGE;
            int hours = sliderValue / TIME_CHANGES_PER_HOUR;
            return hours * 100 + minutes + MINIMUM_START_TIME;
        }

        static int ConvertTimeToSliderValue(int time)
        {
            int hours = (time - MINIMUM_START_TIME) / 100;
            int minutes = time % 100;
            return hours * TIME_CHANGES_PER_HOUR + minutes / MINUTES_PER_TIME_CHANGE;
        }

        public OptionSliderTimePicker(string label, int initialTimeSelected, Action<int> timeSelectedChangedCallback, int minTime = MINIMUM_START_TIME, int maxTime = MAXIMUM_END_TIME, int stepInMinutes = MINUTES_PER_TIME_CHANGE)
            : base(label, ConvertTimeToSliderValue(initialTimeSelected), newValue => timeSelectedChangedCallback(ConvertSliderValueToTime(newValue)))
        {
            if (minTime < MINIMUM_START_TIME)
            {
                throw new ArgumentException(String.Format("Minimum time cannot be less than {0} got {1}", MINIMUM_START_TIME, minTime));
            } else if (maxTime > MAXIMUM_END_TIME)
            {
                throw new ArgumentException(String.Format("Maximum time cannot be greater than {0} got {1}", MAXIMUM_END_TIME, maxTime));
            } else if (minTime >= maxTime)
            {
                throw new ArgumentException(String.Format("Minimum time must be less than Maximum time, got {0}, {1}", minTime, maxTime));
            } else if (stepInMinutes % MINUTES_PER_TIME_CHANGE != 0)
            {
                throw new ArgumentException(String.Format("Step must be divisible by {0}. Which is the smallest about of time that passes, got {1}", MINUTES_PER_TIME_CHANGE, stepInMinutes));
            } else if (stepInMinutes <= 0)
            {
                throw new ArgumentException(String.Format("Step must be positive, got {0}", stepInMinutes));
            }

            this.MinValue = ConvertTimeToSliderValue(minTime);
            this.MaxValue = ConvertTimeToSliderValue(maxTime);
            this.Step = stepInMinutes / MINUTES_PER_TIME_CHANGE;
        }

        public int TimeSelected
        {
            get => ConvertSliderValueToTime(this.Value);
            set => this.Value = ConvertTimeToSliderValue(value);
        }

        public override string ConvertValueToString(int value)
        {
            return StardewValley.Game1.getTimeOfDayString(ConvertSliderValueToTime(value));
        }
    }
}
