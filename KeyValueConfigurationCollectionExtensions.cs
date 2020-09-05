/*
	C# "KeyValueConfigurationCollectionExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Configuration;

namespace ReSearcher {

	public static class KeyValueConfigurationCollectionExtensions {

		#region retrieving

			public static String getValueOr(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String defaultValue, String key) {
				KeyValueConfigurationElement keyValueConfigurationElement = thisKeyValueConfigurationCollection[key];
				return(null == keyValueConfigurationElement ? defaultValue : keyValueConfigurationElement.Value);
			}

			public static String getValueOrNull(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String key) {
				return(thisKeyValueConfigurationCollection.getValueOr(null, key));
			}

			public static KeyValueConfigurationCollection take(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String key, out String value) {
				value = thisKeyValueConfigurationCollection.getValueOrNull(key);
				return(thisKeyValueConfigurationCollection);
			}

			public static KeyValueConfigurationCollection take(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String key, out String value, String defaultValue) {
				value = thisKeyValueConfigurationCollection.getValueOrNull(key) ?? defaultValue;
				return(thisKeyValueConfigurationCollection);
			}

			public static KeyValueConfigurationCollection take(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String key, out String value, Func<String> defaultValueProvider) {
				value = thisKeyValueConfigurationCollection.getValueOrNull(key) ?? defaultValueProvider();
				return(thisKeyValueConfigurationCollection);
			}

		#endregion

		#region putting

			public static void put(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String key, String value) {
				KeyValueConfigurationElement keyValueConfigurationElement = thisKeyValueConfigurationCollection[key];
				if(null == keyValueConfigurationElement) {
					keyValueConfigurationElement = new KeyValueConfigurationElement(key, value);
					thisKeyValueConfigurationCollection.Add(keyValueConfigurationElement);
				}
				else {
					keyValueConfigurationElement.Value = value;
				}
			}

			public static KeyValueConfigurationCollection with(this KeyValueConfigurationCollection thisKeyValueConfigurationCollection, String key, String value) {
				thisKeyValueConfigurationCollection.put(key, value);
				return(thisKeyValueConfigurationCollection);
			}

		#endregion

	}

}