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
using Articy.Unity.Constraints;
using Articy.ManiacManfred;

namespace Articy.ManiacManfred.Features
{
    
    
    public class DisplayConditionFeatureConstraint
    {
        
        private Boolean mLoadedConstraints;
        
        private ScriptConstraint mShowMeIf;
        
        public ScriptConstraint ShowMeIf
        {
            get
            {
                EnsureConstraints();
                return mShowMeIf;
            }
        }
        
        public virtual void EnsureConstraints()
        {
            if ((mLoadedConstraints == true))
            {
                return;
            }
            mLoadedConstraints = true;
            mShowMeIf = new Articy.Unity.Constraints.ScriptConstraint("Condition", "");
        }
    }
}
