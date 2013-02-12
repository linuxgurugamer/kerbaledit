// -----------------------------------------------------------------------
// <copyright file="KerbalDataViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class KerbalDataViewModel
    {

        private readonly ReadOnlyCollection<TreeViewItemViewModel> objects; //= new ReadOnlyCollection<string>(new List<string>() { "Save Games", "Scenarios", "Training Scenarios", "VAB Craft", "SPH Craft", "Settings/Configuration" });
        private KerbalData kd;

        /// Initializes a new instance of the <see cref="KerbalDataViewModel" /> class.
        /// </summary>	
        public KerbalDataViewModel(KerbalData kd)
        {
            this.kd = kd;

            objects = new ReadOnlyCollection<TreeViewItemViewModel>(new List<TreeViewItemViewModel>()
            {
                new StorableObjectsViewModel<SaveFile>(kd.Saves, "Saves"),
                new StorableObjectsViewModel<SaveFile>(kd.Scenarios, "Scenarios"),
                new StorableObjectsViewModel<SaveFile>(kd.TrainingScenarios, "TrainingScenarios"),
                new StorableObjectsViewModel<CraftFile>(kd.CraftInVab, "CraftInVab"),
                new StorableObjectsViewModel<CraftFile>(kd.CraftInSph, "CraftInSph"),
                new StorableObjectsViewModel<PartFile>(kd.Parts, "Parts"),
                new StorableObjectsViewModel<ConfigFile>(kd.KspSettings, "KspSettings")
            });

        }

        public ReadOnlyCollection<TreeViewItemViewModel> Objects { get { return objects; } }
    }
}
