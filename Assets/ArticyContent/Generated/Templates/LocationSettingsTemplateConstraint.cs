// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Articy.Unity.Interfaces;
using Articy.Unity;
using Articy.ManiacManfred.Features;
using Articy.ManiacManfred;

namespace Articy.ManiacManfred.Templates
{
    
    
    public class LocationSettingsTemplateConstraint
    {
        
        private SoundfileFeatureConstraint mSoundfile = new SoundfileFeatureConstraint();
        
        private LocationSettingsFeatureConstraint mLocationSettings = new LocationSettingsFeatureConstraint();
        
        public SoundfileFeatureConstraint Soundfile
        {
            get
            {
                return mSoundfile;
            }
        }
        
        public LocationSettingsFeatureConstraint LocationSettings
        {
            get
            {
                return mLocationSettings;
            }
        }
    }
}