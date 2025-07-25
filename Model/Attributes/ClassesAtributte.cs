namespace Model.Attributes
{
    public class ClassesAtributte : Attribute
    {
        public string Classes { get; set; }

        public ClassesAtributte(string classes)
        {
            Classes = classes;
        }
    }
}