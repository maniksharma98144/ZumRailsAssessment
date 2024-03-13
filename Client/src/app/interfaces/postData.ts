export default interface PostData {
    id: number,
    authorId: number,
    author: string,
    likes: number,
    popularity: number,
    reads: number,
    tags: string[]
}