using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = SectionTypeArrayToList(sections);
        }

        public LinkedList<Section> SectionTypeArrayToList(SectionTypes[] sectionTypesArray)
        {
            var result = new  LinkedList<Section>();

            foreach (SectionTypes sectionType in sectionTypesArray)
            {
                var temp = new Section();
                temp.SectionType = sectionType;
                result.AddLast(temp);
            }

            return result;
        }
    }
}


public enum SectionTypes
{
    StartGrid,
    Straight,
    LeftCorner,
    RightCorner,
    Finish
}