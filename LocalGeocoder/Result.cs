namespace LocalGeocoder
{
    public struct Result
    {
        private readonly IdName _country;
        private readonly IdName _administativeArea1;
        private readonly IdName _administativeArea2;

        public struct IdName
        {
            private readonly string _id;
            private readonly string _name;

            internal IdName(Entity entity)
            {
                _id = entity.Id;
                _name = entity.Name;
            }

            public string Id
            {
                get { return _id; }
            }

            public string Name
            {
                get { return _name; }
            }

            public override string ToString()
            {
                return string.IsNullOrEmpty(_id) ? _name : string.Format("{0} ({1})", _name, _id);
            }
        }

        internal Result(Entity country, Entity administrativeArea1, Entity administrativeArea2)
        {
            _country = new IdName(country);
            _administativeArea1 = new IdName(administrativeArea1);
            _administativeArea2 = new IdName(administrativeArea2);
        }

        public IdName Country
        {
            get { return _country; }
        }

        public IdName AdministativeArea1
        {
            get { return _administativeArea1; }
        }

        public IdName AdministativeArea2
        {
            get { return _administativeArea2; }
        }

        public override string ToString()
        {
            return string.Join(", ", _country, _administativeArea1, _administativeArea2);
        }
    }
}