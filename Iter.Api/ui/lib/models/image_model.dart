class ImageModel {
  String? image;
  String? name;

  ImageModel({this.image, this.name});

  Map<String, dynamic> toJson() {
    return {
      'image': image,
      'name': name,
    };
  }
}
