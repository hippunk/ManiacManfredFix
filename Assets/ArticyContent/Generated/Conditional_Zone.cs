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
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.ManiacManfred.Features;

namespace Articy.ManiacManfred
{
    
    
    public class Conditional_Zone : Zone, IZone, IPropertyProvider, IObjectWithFeatureZoneCondition
    {
        
        [SerializeField()]
        private ArticyValueConditional_ZoneTemplate mTemplate = new ArticyValueConditional_ZoneTemplate();
        
        private static Articy.ManiacManfred.Templates.Conditional_ZoneTemplateConstraint mConstraints = new Articy.ManiacManfred.Templates.Conditional_ZoneTemplateConstraint();
        
        public Articy.ManiacManfred.Templates.Conditional_ZoneTemplate Template
        {
            get
            {
                return mTemplate.GetValue();
            }
            set
            {
                mTemplate.SetValue(value);
            }
        }
        
        public static Articy.ManiacManfred.Templates.Conditional_ZoneTemplateConstraint Constraints
        {
            get
            {
                return mConstraints;
            }
        }
        
        public ZoneConditionFeature GetFeatureZoneCondition()
        {
            return Template.ZoneCondition;
        }
        
        protected override void CloneProperties(object aClone)
        {
            Conditional_Zone newClone = ((Conditional_Zone)(aClone));
            if ((Template != null))
            {
                newClone.Template = ((Articy.ManiacManfred.Templates.Conditional_ZoneTemplate)(Template.CloneObject()));
            }
            base.CloneProperties(newClone);
        }
        
        public override bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return base.IsLocalizedPropertyOverwritten(aProperty);
        }
        
        #region property provider interface
        public override void setProp(string aProperty, object aValue)
        {
            if (aProperty.Contains("."))
            {
                Template.setProp(aProperty, aValue);
                return;
            }
            base.setProp(aProperty, aValue);
        }
        
        public override Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if (aProperty.Contains("."))
            {
                return Template.getProp(aProperty);
            }
            return base.getProp(aProperty);
        }
        #endregion
    }
}
