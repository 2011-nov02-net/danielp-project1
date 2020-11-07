namespace MyStore.Store
{
    public class Item
    {
        float cost;
        string name;


        //COULD ALSO NOT NEED THIS IF WE HAVE ONE INSTANCE OF EACH ITEM 
        //doing these overrides means we can have multiple objects in memory w/ same name
        //not doing this means we would probably 
        //https://ericlippert.com/2011/02/28/guidelines-and-rules-for-gethashcode/

        //should not be different if the object is the same
        // -> should be based on immuteable fields of the object
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is Item)
            {
                //check if names are equal
            }
            return base.Equals(obj);
        }
    }
}