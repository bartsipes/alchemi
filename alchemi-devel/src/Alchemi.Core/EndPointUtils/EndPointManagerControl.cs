using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Alchemi.Core.EndPointUtils
{
    /// <summary>
    /// The control for managing a collection of end points.
    /// </summary>
    public partial class EndPointManagerControl : UserControl
    {
        #region Constructor
        public EndPointManagerControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Members
        private Alchemi.Core.EndPointUtils.EndPointConfiguration currentEndPointConfiuration = null;
        private string currentEndPointPreviousName = string.Empty;
        #endregion

        #region Properties

        #region EndPoints
        private EndPointConfigurationCollection _EndPoints = null;
        /// <summary>
        /// The collection of end points to set.
        /// </summary>
        public EndPointConfigurationCollection EndPoints
        {
            get 
            {
                return _EndPoints;
            }
            set 
            {
                _EndPoints = value;
                RebindEndPointsList();
            }
        }
        #endregion

        #endregion

        #region Methods

        #region RebindEndPointsList
        private void RebindEndPointsList()
        {
            if (EndPoints == null)
                return;

            lbEndPointList.Items.Clear();
            foreach (string key in EndPoints.Keys)
            {
                Alchemi.Core.EndPointUtils.EndPointConfiguration epc = EndPoints[key];
                epc.FriendlyName = key;
                lbEndPointList.Items.Add(epc);
            }
            btnDelete.Enabled = false;
        }
        #endregion

        #region GetNewEPName
        private string GetNewEPName()
        {
            if (EndPoints == null)
                return string.Empty;

            int numCandidate = 1;
            string nameCandidate = String.Format("endPoint{0}", numCandidate.ToString());

            while (EndPoints.ContainsKey(nameCandidate))
            {
                numCandidate++;
                nameCandidate = String.Format("endPoint{0}", numCandidate.ToString());
            }

            return nameCandidate;
        }
        #endregion

        #endregion

        #region Handlers

        #region btnNew_Click
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (EndPoints == null)
                return;

            currentEndPointConfiuration = new Alchemi.Core.EndPointUtils.EndPointConfiguration(Alchemi.Core.Utility.AlchemiRole.Manager);
            currentEndPointPreviousName = string.Empty;

            ucEndPointConfig.Clear();
            ucEndPointConfig.AddressPart = "Manager";
            ucEndPointConfig.Enabled = true;
            btnSave.Enabled = true;
            txtEPName.Enabled = true;
            txtEPName.Text = GetNewEPName();
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (EndPoints == null)
                return;
            ucEndPointConfig.WriteEndPointConfiguration(currentEndPointConfiuration);

            //remove previous input of this object
            if (currentEndPointPreviousName != string.Empty)
                EndPoints.Remove(currentEndPointPreviousName);

            currentEndPointConfiuration.FriendlyName = txtEPName.Text;
            EndPoints.Add(txtEPName.Text, currentEndPointConfiuration);

            RebindEndPointsList();

            ucEndPointConfig.Clear();
            ucEndPointConfig.AddressPart = "Manager";
            ucEndPointConfig.Enabled = false;
            btnSave.Enabled = false;
            txtEPName.Enabled = false;
            txtEPName.Text = string.Empty;
        }
        #endregion

        #region btnDelete_Click
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (EndPoints == null)
                return;

            Alchemi.Core.EndPointUtils.EndPointConfiguration epc = lbEndPointList.SelectedItem as Alchemi.Core.EndPointUtils.EndPointConfiguration;

            if (epc != null && EndPoints.ContainsKey(epc.ToString()))
                EndPoints.Remove(epc.ToString());

            RebindEndPointsList();

            ucEndPointConfig.Clear();
            ucEndPointConfig.AddressPart = "Manager";
            ucEndPointConfig.Enabled = false;
            lbEndPointList.ClearSelected();
            txtEPName.Enabled = false;
            txtEPName.Text = string.Empty;
        }
        #endregion

        #region lbEndPoints_SelectedIndexChanged
        private void lbEndPoints_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (EndPoints == null)
                return;

            currentEndPointConfiuration = lbEndPointList.SelectedItem as Alchemi.Core.EndPointUtils.EndPointConfiguration;
            currentEndPointPreviousName = currentEndPointConfiuration.ToString();

            ucEndPointConfig.ReadEndPointConfiguration(currentEndPointConfiuration);
            txtEPName.Text = currentEndPointConfiuration.ToString();

            if (lbEndPointList.SelectedItem != null)
            {
                btnDelete.Enabled = true;
                ucEndPointConfig.Enabled = true;
                btnSave.Enabled = true;
                txtEPName.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                ucEndPointConfig.Enabled = false;
                btnSave.Enabled = false;
                txtEPName.Enabled = false;
            }
        }
        #endregion

        #endregion
    }
}
