using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Helpers;
using VisualGrading.Settings;

namespace StudentTestReporting.Helpers
{
    public abstract class AbstractRepository
    {
        internal SettingsProfile SettingsProfile
        {
            get
            {
                return SettingsProfile.Instance;
            }
        }

        internal string ClassName
        {
            get
            {
                return this.GetType().ToString();
            }
        }
    }
}
