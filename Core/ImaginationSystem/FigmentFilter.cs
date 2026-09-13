namespace Trimaw.Core.ImaginationSystem;

public class FigmentFilter
{
    private readonly Criterion[] _criteria;

    private FigmentFilter(List<Criterion> criteria)
    {
        _criteria = [.. criteria];
    }

    public static Builder All => new();

    public decimal GetMult(TaggedFigment figment)
    {
        var m = _criteria.Aggregate(1.0M, (mult, c) =>
            mult * (c.FilterTags.Any(figment.Tags.Contains) ? c.AnyMult : c.NoneMult));
        return Math.Max(0.0M, m);
    }

    private record Criterion(IReadOnlyList<Tag> FilterTags, decimal AnyMult, decimal NoneMult);

    public class Builder
    {
        private readonly List<Criterion> _criteria = [];

        public static implicit operator FigmentFilter(Builder builder)
        {
            return new FigmentFilter(builder._criteria);
        }

        public Builder Applying(FigmentFilter other)
        {
            _criteria.AddRange(other._criteria);
            return this;
        }

        public Builder Multiplying(Tag tag, decimal mult)
        {
            _criteria.Add(new Criterion([tag], mult, 1));
            return this;
        }

        public Builder Requiring(Tag tag)
        {
            _criteria.Add(new Criterion([tag], 1, 0));
            return this;
        }

        public Builder RequiringAny(params Tag[] tags)
        {
            _criteria.Add(new Criterion(tags, 1, 0));
            return this;
        }

        public Builder Forbidding(Tag tag)
        {
            _criteria.Add(new Criterion([tag], 0, 1));
            return this;
        }

        public Builder ForbiddingEach(params Tag[] tags)
        {
            _criteria.Add(new Criterion(tags, 0, 1));
            return this;
        }
    }

    public readonly struct Tag : IEquatable<Tag>
    {
        private readonly int _value;

        private Tag(int value)
        {
            _value = value;
        }

        public static implicit operator Tag(HardTag tag)
        {
            return new Tag((ushort)tag);
        }

        public static implicit operator Tag(SoftTag tag)
        {
            return new Tag((ushort)tag << 16);
        }

        public bool Equals(Tag other)
        {
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Tag other && Equals(other);
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return _value;
        }
    }
}