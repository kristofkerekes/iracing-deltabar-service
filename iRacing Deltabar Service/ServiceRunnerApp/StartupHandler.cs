using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RunOnStartup {
    public class Startup {
        public static bool RunOnStartup() {
            return RunOnStartup(Application.ProductName, Application.ExecutablePath);
        }

        public static bool RunOnStartup(string AppTitle, string AppPath) {
            RegistryKey rk;
            try {
                rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                rk.SetValue(AppTitle, AppPath);
                return true;
            } catch (Exception) {
            }

            try {
                rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                rk.SetValue(AppTitle, AppPath);
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public static bool RemoveFromStartup() {
            return RemoveFromStartup(Application.ProductName, Application.ExecutablePath);
        }

        public static bool RemoveFromStartup(string AppTitle) {
            return RemoveFromStartup(AppTitle, null);
        }

        public static bool RemoveFromStartup(string AppTitle, string AppPath) {
            RegistryKey rk;
            try {
                rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (AppPath == null) {
                    rk.DeleteValue(AppTitle);
                } else {
                    if (rk.GetValue(AppTitle).ToString().ToLower() == AppPath.ToLower()) {
                        rk.DeleteValue(AppTitle);
                    }
                }
                return true;
            } catch (Exception) {
            }

            try {
                rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (AppPath == null) {
                    rk.DeleteValue(AppTitle);
                } else {
                    if (rk.GetValue(AppTitle).ToString().ToLower() == AppPath.ToLower()) {
                        rk.DeleteValue(AppTitle);
                    }
                }
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public static bool IsInStartup() {
            return IsInStartup(Application.ProductName, Application.ExecutablePath);
        }

        public static bool IsInStartup(string AppTitle, string AppPath) {
            RegistryKey rk;
            string value;

            try {
                rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                value = rk.GetValue(AppTitle).ToString();
                if (value == null) {
                    return false;
                } else if (!value.ToLower().Equals(AppPath.ToLower())) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception) {
            }

            try {
                rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                value = rk.GetValue(AppTitle).ToString();
                if (value == null) {
                    return false;
                } else if (!value.ToLower().Equals(AppPath.ToLower())) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception) {
            }

            return false;
        }
    }
}
