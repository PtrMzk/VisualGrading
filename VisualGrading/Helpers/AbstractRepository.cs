using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Helpers;

namespace StudentTestReporting.Helpers
{
    public abstract class AbstractRepository
    {
        internal SettingRepository settingRepository
        {
            get
            {
                return SettingRepository.Instance;
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
