import { Component, Input } from '@angular/core';
import { GitHubCommitDto } from '../../../shared/models/git-commits.dto';

@Component({
  selector: 'app-release-note-card',
  templateUrl: './release-note-card.component.html',
  styleUrl: './release-note-card.component.scss'
})
export class ReleaseNoteCardComponent {
  @Input() commitData: GitHubCommitDto;
}
