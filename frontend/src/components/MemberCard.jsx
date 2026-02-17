export default function MemberCard({ member, onRemove }) {
  return (
    <div className="member-card">
      <div className="member-card-header">
        <h4>{member.name}</h4>
        <button
          className="btn btn-remove"
          onClick={onRemove}
          aria-label={`Remove ${member.name}`}
        >
          Remove
        </button>
      </div>
      <div className="member-card-details">
        <span>{member.age} years old</span>
        <span className="detail-sep">|</span>
        <span>{member.sex}</span>
        <span className="detail-sep">|</span>
        <span>{member.heightCm} cm, {member.weightKg} kg</span>
        <span className="detail-sep">|</span>
        <span>{member.activityLevel}</span>
      </div>
      {member.allergies.length > 0 && (
        <div className="member-card-tags">
          <strong>Allergies:</strong>{' '}
          {member.allergies.map((a, i) => (
            <span key={i} className="tag tag-allergy">{a}</span>
          ))}
        </div>
      )}
      {member.likes.length > 0 && (
        <div className="member-card-tags">
          <strong>Likes:</strong>{' '}
          {member.likes.map((l, i) => (
            <span key={i} className="tag tag-like">{l}</span>
          ))}
        </div>
      )}
      {member.dislikes.length > 0 && (
        <div className="member-card-tags">
          <strong>Dislikes:</strong>{' '}
          {member.dislikes.map((d, i) => (
            <span key={i} className="tag tag-dislike">{d}</span>
          ))}
        </div>
      )}
    </div>
  );
}
