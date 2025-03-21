public class Edge {

  public Vertex from;
  public Vertex to;
  public int cost;

  public Edge(Vertex from, Vertex to, int cost)
  {
    this.from = from;
    this.to = to;
    this.cost = cost;
  }

  public Edge(Vertex from, Vertex to) : this(from,to, 1) {}

  public override bool Equals(object obj)
  {
    if (obj is Edge other)
    {
      return ( from == other.from && to == other.to ) || 
        ( from == other.to && to == other.from );
    }
    return false;
  }

  public override int GetHashCode()
  {
    int hash1 = from.GetHashCode();
    int hash2 = to.GetHashCode();
    return hash1 ^ hash2;
  }
}