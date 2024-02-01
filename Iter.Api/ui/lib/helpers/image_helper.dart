import 'dart:io';
import 'dart:convert';
import 'package:flutter_image_compress/flutter_image_compress.dart';
import 'package:path/path.dart';
import 'package:ui/models/image_model.dart';

class ImageHelper {
  static Future<List<int>> generateThumbnail(File imageFile) async {
    var thumbnailBytes = await FlutterImageCompress.compressWithFile(
      imageFile.absolute.path,
      minWidth: 100,
      minHeight: 100,
      quality: 70,
    );
    return thumbnailBytes ?? [];
  }

  static Future<ImageModel> processImage(File imageFile) async {
    var imageBytes = await imageFile.readAsBytes();
    String base64Image = base64Encode(imageBytes);

    // List<int> thumbnailBytes = await generateThumbnail(imageFile);
    // String base64Thumbnail = base64Encode(thumbnailBytes);

    return ImageModel(image: base64Image, name: basename(imageFile.path));
  }
}