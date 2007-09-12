using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core;
using System.Collections;
using NPlot.Windows;
using Alchemi.Core.Manager.Storage;
using System.Drawing.Drawing2D;
using Alchemi.Core.Utility;
using NPlot;

namespace Alchemi.Console.PropertiesDialogs
{
    public partial class ExecutorProperties : PropertiesForm
    {
        // Create a logger for use in this class
        private static readonly Logger logger = new Logger();

        private ConsoleNode console;

        public ExecutorProperties(ConsoleNode console)
        {
            InitializeComponent();

            this.console = console;
        }


        #region Graph Stuff

        //For the summary graph
        private ArrayList x1 = new ArrayList();
        private ArrayList y1 = new ArrayList();
        private ArrayList y2 = new ArrayList();
        private double xVal = -1;
        private LinePlot lineUsage = new LinePlot();
        private LinePlot lineAvail = new LinePlot();

        private void InitSystemPlot()
        {
            try
            {
                plotSurface.Clear();
                this.plotSurface.RightMenu = NPlot.Windows.PlotSurface2D.DefaultContextMenu;
                
                plotSurface.Add(lineAvail);
                plotSurface.Add(lineUsage);

                plotSurface.PlotBackColor = plotSurface.BackColor;
                plotSurface.SmoothingMode = SmoothingMode.AntiAlias;

                plotSurface.Title = "CPU Power - Availability & Usage";
                plotSurface.TitleFont = new Font(new FontFamily("Microsoft Sans Serif"), 6.5f, FontStyle.Regular);

                plotSurface.XAxis1.WorldMin = -60.0f;
                plotSurface.XAxis1.WorldMax = 0.0f;
                //plotSurface.XAxis1.Label = "Seconds";
                //plotSurface.XAxis1.LabelFont = new Font(new FontFamily("Microsoft Sans Serif" ), 6.0f, FontStyle.Regular);
                //plotSurface.XAxis1.TickTextFont = new Font(new FontFamily("Microsoft Sans Serif" ), 6.0f, FontStyle.Regular);

                plotSurface.YAxis1.WorldMin = 0.0;
                plotSurface.YAxis1.WorldMax = 100.0;
                //plotSurface.YAxis1.Label = "Power [%]";
                //plotSurface.YAxis1.LabelFont = new Font(new FontFamily("Microsoft Sans Serif" ), 6.0f, FontStyle.Regular);
                //plotSurface.YAxis1.TickTextFont = new Font(new FontFamily("Microsoft Sans Serif" ), 6.0f, FontStyle.Regular);

                Grid gridPlotSurface = new Grid();
                gridPlotSurface.HorizontalGridType = Grid.GridType.None;
                gridPlotSurface.VerticalGridType = Grid.GridType.Fine;
                gridPlotSurface.MajorGridPen.Color = Color.DarkGray;
                plotSurface.Add(gridPlotSurface);

                //				plotSurface.Legend = new Legend();
                //				plotSurface.Legend.Font = new Font(new FontFamily("Microsoft Sans Serif" ), 6.0f, FontStyle.Regular);
                //				plotSurface.Legend.NeverShiftAxes = false;
                //				plotSurface.Legend.AttachTo( NPlot.PlotSurface2D.XAxisPosition.Top , NPlot.PlotSurface2D.YAxisPosition.Right);
                //				plotSurface.Legend.HorizontalEdgePlacement = Legend.Placement.Inside;
                //				plotSurface.Legend.VerticalEdgePlacement = Legend.Placement.Inside;

                //lineAvail.Label = "usage";
                lineAvail.Pen = new Pen(Color.LightBlue, 1.7f);

                //lineUsage.Label = "avail";
                lineUsage.Pen = new Pen(Color.LightGreen, 1.7f);

                plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.HorizontalDrag());
                plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.VerticalDrag());
                plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.AxisDrag(true));

                plotSurface.PlotBackColor = Color.Black;
                plotSurface.BackColor = SystemColors.Control;
                plotSurface.XAxis1.Color = Color.Black;
                plotSurface.YAxis1.Color = Color.Black;

                plotSurface.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldnot initialize graph. Error: " + ex.Message, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshSystemPlot()
        {
            try
            {
                SystemSummary summary = console.Manager.Admon_GetSystemSummary(console.Credentials);

                if (summary == null)
                {
                    logger.Debug("Summary is null!");
                }
                else
                {
                    xVal++;

                    x1.Add(xVal);

                    y1.Add(Convert.ToDouble(summary.PowerUsage));
                    y2.Add(Convert.ToDouble(summary.PowerAvailable));

                    if (x1.Count > 31)
                    {
                        x1.RemoveAt(0);
                        y1.RemoveAt(0);
                        y2.RemoveAt(0);
                    }


                    int npt = 31;
                    int[] xTime = new int[npt];
                    double[] yAvail = new double[npt];
                    double[] yUsage = new double[npt];

                    for (int i = 0; i < x1.Count; i++)
                    {
                        int x2 = ((((31 - x1.Count) + i)) * 2) - 60;
                        xTime[i] = x2;
                        yAvail[i] = (double)y1[i];
                        yUsage[i] = (double)y2[i];
                    }

                    lineAvail.AbscissaData = xTime;
                    lineAvail.OrdinateData = yAvail;

                    lineUsage.AbscissaData = xTime;
                    lineUsage.OrdinateData = yUsage;

                    plotSurface.Refresh();

                }
            }
            catch (Exception ex)
            {
                logger.Error("Could not refresh system. Error: ", ex);
            }
        }


        private void tmRefreshSystem_Tick(object sender, EventArgs e)
        {
            RefreshData(null); // TODO: WTF?  Why even bother...it just returns if null.
            RefreshSystemPlot();
        }

        #endregion

        private void RefreshData(ExecutorStorageView ex)
        {
            if (ex == null)
                return;

            txId.Text = ex.ExecutorId;
            lbName.Text = ex.HostName;

            this.Text = ex.HostName + " Properties";

            txPort.Text = ex.Port.ToString();
            txUsername.Text = ex.Username;

            chkConnected.Checked = ex.Connected;
            chkDedicated.Checked = ex.Dedicated;

            txArch.Text = ex.Architecture;
            txOS.Text = ex.OS;

            txMaxCPU.Text = ex.MaxCpu.ToString();
            txMaxDisk.Text = ex.MaxDisk.ToString();
            txNumCPUs.Text = ex.NumberOfCpu.ToString();

            txPingTime.Text = GetDiff(ex.PingTime);
        }

        public void SetData(ExecutorStorageView ex)
        {
            RefreshData(ex);
            InitSystemPlot();
            RefreshSystemPlot();
            tmRefreshSystem.Enabled = true;
        }

        private string GetDiff(DateTime d)
        {
            string diff = "unknown";
            double difference = Utils.DateDiff(DateTimeInterval.Minute, DateTime.Now, d);
            if (difference > 0)
            {
                diff = difference.ToString("F") + " mins. ago";
            }
            return diff;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}