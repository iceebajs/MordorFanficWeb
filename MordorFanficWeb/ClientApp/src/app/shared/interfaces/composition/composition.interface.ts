import { Chapter } from './../chapter/chapter.interface';

export interface Composition{
  compositionId: number;
  title: string;
  previewContext: string;
  genre: string;
  tags: string;
  userId: number;
  chapters: Chapter[];
}
