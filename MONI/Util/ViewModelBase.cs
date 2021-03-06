using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace MONI.Util
{
  public class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Called when [property changed].
    /// </summary>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    /// <param name="projection">The projection.</param>
    public void OnPropertyChanged<TPropertyType>(Expression<Func<TPropertyType>> projection) {
      var tmp = this.PropertyChanged;
      if (tmp != null) {
        if (projection != null) {
          CheckIfExpressionValid(projection);
          tmp(this, new PropertyChangedEventArgs(PropertyName(projection)));
        } else {
          tmp(this, new PropertyChangedEventArgs(String.Empty));
        }
      }
    }

    public void OnPropertyChanged(string thePropertyName) {
      var tmp = this.PropertyChanged;
      if (tmp != null) {
        tmp(this, new PropertyChangedEventArgs(thePropertyName));
      }
    }

    [Conditional("DEBUG")]
    private static void CheckIfExpressionValid<TPropType>(Expression<Func<TPropType>> projection) {
      var memberExpression = (MemberExpression)projection.Body;

      if ((memberExpression.Member.MemberType & MemberTypes.Property) != MemberTypes.Property) {
        throw new ArgumentException("Not a Property!", "projection");
      }
    }

    /// <summary>
    /// Gets the Property name in a type-safe way, used for PropertyChanged-Events.
    /// </summary>
    /// <typeparam name="TPropType">the return type of the property.</typeparam>
    /// <param name="projection">The property-projection, s.th. like x=&gt;x.PropertyName.</param>
    /// <returns>the PropertyName.</returns>
    private static string PropertyName<TPropType>(Expression<Func<TPropType>> projection) {
      var memberExpression = (MemberExpression)projection.Body;
      return memberExpression.Member.Name;
    }
  }
}