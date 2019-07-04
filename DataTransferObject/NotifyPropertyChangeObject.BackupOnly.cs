using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq.Expressions;


namespace DataTransferObject
{

    #region About this
    //A little word about the code

    //Our class Person inherits from NotifyPropertyChangeObject.
    //When we change the value of a property, the magic happens.
    //We forward the private variable, the (refactor friendly) property, and the new value.
    //After that, we check if the ‘new’ value matches the old one. If this is the case, nothing should happen.
    //If the values do not match, we want to change the value and notify that a change has happened.
    //In the meanwhile, we also add that change to the Changes dictionary.
    //It’s safe to presume that if the dictionary contains changes, the object is dirty.
    //Finally, there are the methods Reset, StartTracking, and StopTracking. These have to do with the change tracking.

    //If I’m filling up a DTO in my DAL, I don’t want it to be marked as dirty. So before I start, I call StopTracking, and when I’m done, I call StartTracking.

    //Later on, if I save my object, I want it to be clean (not dirty), so I call the Reset method.

    //Finally, you could call ChangesToXml to get a string representation of all the changes. This could the be written to SQL or so...

    //Example

    //Reset:        LE MINH HIEP: Need to add this NotifyPropertyChanged: For the purpose of binding object: Always refresh binding when Reset (Atleast one visible object bind to this dirty property)
    //SetDirty:     LE MINH HIEP: call SetDirty from BLL Object to TURN IsDirty ON (value = true) of BLL Object itself WHEN DTO object Is Dirty

    #endregion Aboutthis


    /// <summary>
    /// This object automatically implements notify property changed
    /// </summary>
    public class NotifyPropertyChangeObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Track changes or not.
        /// If we're working with DTOs and we fill up the DTO in the DAL we should not be tracking changes.
        /// </summary>
        private bool trackChanges = false;

        /// <summary>
        /// Changes to the object
        /// </summary>
        public Dictionary<string, object> Changes { get; private set; }

        /// <summary>
        /// Is the object dirty or not?
        /// </summary>
        public bool IsDirty
        {
            get { return Changes.Count > 0; }
            //set { ; }
        }

        /// <summary>
        /// Event required for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This constructor will initialize the change tracking
        /// </summary>
        public NotifyPropertyChangeObject()
        {
            // Change tracking default
            trackChanges = true;

            // New change tracking dictionary
            Changes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Reset the object to non-dirty
        /// </summary>
        public void Reset()
        {
            Changes.Clear();
            NotifyPropertyChanged("IsDirty");//LE MINH HIEP: Need to add this NotifyPropertyChanged: For the purpose of binding object: Always refresh binding when Reset (Atleast one visible object bind to this dirty property)
        }

        //LE MINH HIEP: call SetDirty from BLL Object only to TURN IsDirty ON (value = true) of BLL Object itself WHEN DTO object Is Dirty
        //LE MINH HIEP: ONLY BLL OBJECT NEED TO CALL THIS METHOD to set it IsDirty itself. DTO has never needed this method
        protected void SetDirty()
        {
            if (trackChanges)
            {
                // Change tracking
                Changes["IsDirty"] = true;

                // Notify change
                NotifyPropertyChanged("IsDirty");
            }
        }

        /// <summary>
        /// Start tracking changes
        /// </summary>
        public void StartTracking()
        {
            trackChanges = true;
        }

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public void StopTracking()
        {
            trackChanges = false;
        }

        /// <summary>
        /// Change the property if required and throw event
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected void ApplyPropertyChange<T, F>(ref F field, Expression<Func<T, object>> property, F value)
        {
            // Only do this if the value changes
            if (field == null || !field.Equals(value))
            {
                field = value; // Set the value

                if (trackChanges)   // If change tracking is enabled, we can track the changes...
                {
                    var propertyExpression = GetMemberExpression(property); // Get the property
                    if (propertyExpression == null) throw new InvalidOperationException("You must specify a property");

                    string propertyName = propertyExpression.Member.Name; // Property name

                    Changes[propertyName] = value; // Change tracking

                    NotifyPropertyChanged(propertyName); // Notify change
                }
            }
        }



        /// <summary>
        /// Get member expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        private MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expression)
        {
            // Default expression
            MemberExpression memberExpression = null;

            // Convert
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            // Member access
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            // Not a member access
            if (memberExpression == null)
                throw new ArgumentException("Not a member access", "expression");

            // Return the member expression
            return memberExpression;
        }

        /// <summary>
        /// The property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Convert the changes to an XML string
        /// </summary>
        /// <returns></returns>
        public string ChangesToXml()
        {
            // Prepare base objects
            XDeclaration declaration = new XDeclaration("1.0", Encoding.UTF8.HeaderName, String.Empty);
            XElement root = new XElement("Changes");

            // Create document
            XDocument document = new XDocument(declaration, root);

            // Add changes to the document
            // TODO: If it's an object, maybe do some other things
            foreach (KeyValuePair<string, object> change in Changes)
                root.Add(new XElement(change.Key, change.Value));

            // Get the XML
            return document.Document.ToString();
        }
    }
}


